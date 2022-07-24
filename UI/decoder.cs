using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using FFmpeg.AutoGen;
using FFmpegWraper;

namespace mp4Utl.UI;

/// <summary>
/// 動画デコーダを表現します。
/// </summary>
public unsafe class Decoder : IDisposable
{
    public Decoder()
    {
        ffmpeg.RootPath = @"{DLLのあるディレクトリのパス}";
        ffmpeg.av_register_all();
    }

    private AVFormatContext* formatContext;
    /// <summary>
    /// 現在の <see cref="AVFormatContext"/> を取得します。
    /// </summary>
    public AVFormatContext FormatContext { get => *formatContext; }

    private AVStream* videoStream;
    /// <summary>
    /// 現在の動画ストリームを表す <see cref="AVStream"/> を取得します。
    /// </summary>
    public AVStream VideoStream { get => *videoStream; }

    private AVStream* audioStream;

    private AVCodec* videoCodec;
    /// <summary>
    /// 現在の動画コーデックを表す <see cref="AVCodec"/> を取得します。
    /// </summary>
    public AVCodec VideoCodec { get => *videoCodec; }

    private AVCodec* audioCodec;

    private AVCodecContext* videoCodecContext;
    /// <summary>
    /// 現在の動画コーデックの <see cref="AVCodecContext"/> を取得します。
    /// </summary>
    public AVCodecContext VideoCodecContext { get => *videoCodecContext; }

    private AVCodecContext* audioCodecContext;

    /// <summary>
    /// ファイルを開き、デコーダを初期化します。
    /// </summary>
    /// <param name="path">開くファイルのパス。</param>
    /// <exception cref="InvalidOperationException" />
    public void OpenFile(string path)
    {
        AVFormatContext* _formatContext = null;
        ffmpeg.avformat_open_input(&_formatContext, path, null, null)
            .OnError(() => throw new InvalidOperationException("指定のファイルは開けませんでした。"));
        formatContext = _formatContext;

        ffmpeg.avformat_find_stream_info(formatContext, null)
            .OnError(() => throw new InvalidOperationException("ストリームを検出できませんでした。"));

        videoStream = GetFirstVideoStream();
        audioStream = GetFirstAudioStream();

        if (videoStream is not null)
        {
            videoCodec = ffmpeg.avcodec_find_decoder(videoStream->codecpar->codec_id);
            if (videoCodec is null)
            {
                throw new InvalidOperationException("必要な動画デコーダを検出できませんでした。");
            }

            videoCodecContext = ffmpeg.avcodec_alloc_context3(videoCodec);
            if (videoCodecContext is null)
            {
                throw new InvalidOperationException("動画コーデックのCodecContextの確保に失敗しました。");
            }
            ffmpeg.avcodec_parameters_to_context(videoCodecContext, videoStream->codecpar)
                .OnError(() => throw new InvalidOperationException("動画コーデックパラメータの設定に失敗しました。"));
            ffmpeg.avcodec_open2(videoCodecContext, videoCodec, null)
                .OnError(() => throw new InvalidOperationException("動画コーデックの初期化に失敗しました。"));
        }
        if (audioStream is not null)
        {
            audioCodec = ffmpeg.avcodec_find_decoder(audioStream->codecpar->codec_id);
            if (audioCodec is null)
            {
                throw new InvalidOperationException("必要な音声デコーダを検出できませんでした。");
            }

            audioCodecContext = ffmpeg.avcodec_alloc_context3(audioCodec);
            if (audioCodecContext is null)
            {
                throw new InvalidOperationException("音声コーデックのCodecContextの確保に失敗しました。");
            }
            ffmpeg.avcodec_parameters_to_context(audioCodecContext, audioStream->codecpar)
                .OnError(() => throw new InvalidOperationException("音声コーデックのパラメータ設定に失敗しました。"));
            ffmpeg.avcodec_open2(audioCodecContext, audioCodec, null)
                .OnError(() => throw new InvalidOperationException("音声コーデックの初期化に失敗しました。"));
        }
    }

    private AVStream* GetFirstVideoStream()
    {
        for (int i = 0; i < (int)formatContext->nb_streams; ++i)
        {
            var stream = formatContext->streams[i];
            if (stream->codecpar->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
            {
                return stream;
            }
        }
        return null;
    }

    private AVStream* GetFirstAudioStream()
    {
        for (int i = 0; i < (int)formatContext->nb_streams; i++)
        {
            var stream = formatContext->streams[i];
            if (stream->codecpar->codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO)
            {
                return stream;
            }
        }
        return null;
    }

    private struct AVPacketPtr
    {
        public AVPacket* Ptr;
        public AVPacketPtr(AVPacket* ptr)
        {
            Ptr = ptr;
        }
    }

    private object sendPackedSyncObject = new();

    private Queue<AVPacketPtr> videoPackets = new();
    private Queue<AVPacketPtr> audioPackets = new();

    public int SendPacket(int index)
    {
        lock (sendPackedSyncObject)
        {
            if (index == videoStream->index)
            {
                if (videoPackets.TryDequeue(out var ptr))
                {
                    ffmpeg.avcodec_send_packet(videoCodecContext, ptr.Ptr)
                    .OnError(() => throw new InvalidOperationException("動画デコーダへのパケットの送信に失敗しました。"));
                    ffmpeg.av_packet_unref(ptr.Ptr);
                    return 0;
                }
            }
            if (index == audioStream->index)
            {
                if (audioPackets.TryDequeue(out var ptr))
                {
                    ffmpeg.avcodec_send_packet(audioCodecContext, ptr.Ptr)
                    .OnError(() => throw new InvalidOperationException("音声デコーダへのパケットの送信に失敗しました。"));
                    ffmpeg.av_packet_unref(ptr.Ptr);
                    return 0;
                }
            }

            while (true)
            {
                AVPacket packet = new AVPacket();
                var result = ffmpeg.av_read_frame(formatContext, &packet);
                if (result == 0)
                {
                    if (packet.stream_index == videoStream->index)
                    {
                        if (packet.stream_index == index)
                        {
                            ffmpeg.avcodec_send_packet(videoCodecContext, &packet)
                            .OnError(() => throw new InvalidOperationException("動画デコーダへのパケットの送信に失敗しました。"));
                            ffmpeg.av_packet_unref(&packet);
                            return 0;
                        }
                        else
                        {
                            var _packet = ffmpeg.av_packet_clone(&packet);
                            videoPackets.Enqueue(new AVPacketPtr(_packet));
                            continue;
                        }
                    }
                    if (packet.stream_index == audioStream->index)
                    {
                        if (packet.stream_index == index)
                        {
                            ffmpeg.avcodec_send_packet(audioCodecContext, &packet)
                            .OnError(() => throw new InvalidOperationException("音声デコーダへのパケットの送信に失敗しました。"));
                            ffmpeg.av_packet_unref(&packet);
                            return 0;
                        }
                        else
                        {
                            var _packet = ffmpeg.av_packet_clone(&packet);
                            audioPackets.Enqueue(new AVPacketPtr(_packet));
                            continue;
                        }
                    }
                }
                else
                {
                    return -1;
                }
            }
        }
    }

    /// <summary>
    /// 次のフレームを読み取ります。動画の終端に達している場合は <c>null</c> が返されます。
    /// </summary>
    public unsafe ManagedFrame ReadFrame()
    {
        var frame = ReadUnsafeFrame();
        if (frame is null)
        {
            return null;
        }
        return new ManagedFrame(frame);
    }

    private bool isVideoFrameEnded;

    /// <summary>
    /// 次のフレームを読み取ります。動画の終端に達している場合は <c>null</c> が返されます。
    /// </summary>
    /// <remarks>
    /// 取得したフレームは <see cref="ffmpeg.av_frame_free(AVFrame**)"/> を呼び出して手動で解放する必要があることに注意してください。
    /// </remarks>
    /// <returns></returns>
    public unsafe AVFrame* ReadUnsafeFrame()
    {
        AVFrame* frame = ffmpeg.av_frame_alloc();

        if (ffmpeg.avcodec_receive_frame(videoCodecContext, frame) == 0)
        {
            return frame;
        }
        if (isVideoFrameEnded)
        {
            return null;
        }

        int n;
        while ((n = SendPacket(videoStream->index)) == 0)
        {
            if (ffmpeg.avcodec_receive_frame(videoCodecContext, frame) == 0)
            {
                return frame;
            }
            else
            {

            }
        }

        isVideoFrameEnded = true;
        ffmpeg.avcodec_send_packet(videoCodecContext, null)
            .OnError(() => throw new InvalidOperationException("デコーダへのnullパケットの送信に失敗しました。"));
        if (ffmpeg.avcodec_receive_frame(videoCodecContext, frame) == 0)
        {
            return frame;
        }
        return null;
    }

    /// <summary>
    /// 次の音声フレームを読み取ります。動画の終端に達している場合は <c>null</c> が返されます。
    /// </summary>
    public unsafe ManagedFrame ReadAudioFrame()
    {
        var frame = ReadUnsafeAudioFrame();
        if (frame is null)
        {
            return null;
        }
        return new ManagedFrame(frame);
    }

    private bool isAudioFrameEnded;

    /// <summary>
    /// 次の音声フレームを読み取ります。動画の終端に達している場合は <c>null</c> が返されます。
    /// </summary>
    /// <remarks>
    /// 取得したフレームは <see cref="ffmpeg.av_frame_free(AVFrame**)"/> を呼び出して手動で解放する必要があることに注意してください。
    /// </remarks>
    /// <returns></returns>
    public unsafe AVFrame* ReadUnsafeAudioFrame()
    {
        AVFrame* frame = ffmpeg.av_frame_alloc();

        if (ffmpeg.avcodec_receive_frame(audioCodecContext, frame) == 0)
        {
            return frame;
        }
        if (isAudioFrameEnded)
        {
            return null;
        }

        while (SendPacket(audioStream->index) == 0)
        {
            if (ffmpeg.avcodec_receive_frame(audioCodecContext, frame) == 0)
            {
                return frame;
            }
        }

        isAudioFrameEnded = true;
        ffmpeg.avcodec_send_packet(audioCodecContext, null)
            .OnError(() => throw new InvalidOperationException("デコーダへのnullパケットの送信に失敗しました。"));
        if (ffmpeg.avcodec_receive_frame(audioCodecContext, frame) == 0)
        {
            return frame;
        }
        return null;
    }

    ~Decoder()
    {
        DisposeUnManaged();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        DisposeUnManaged();
        GC.SuppressFinalize(this);
    }

    private bool isDisposed = false;
    private void DisposeUnManaged()
    {
        if (isDisposed) { return; }

        AVCodecContext* codecContext = videoCodecContext;
        AVFormatContext* formatContext = this.formatContext;

        ffmpeg.avcodec_free_context(&codecContext);
        ffmpeg.avformat_close_input(&formatContext);

        isDisposed = true;
    }
}

internal static class WrapperHelper
{
    public static int OnError(this int n, Action act)
    {
        if (n < 0)
        {
            var buffer = Marshal.AllocHGlobal(1000);
            string str;
            unsafe
            {
                ffmpeg.av_make_error_string((byte*)buffer.ToPointer(), 1000, n);
                str = new string((sbyte*)buffer.ToPointer());
            }
            Marshal.FreeHGlobal(buffer);
            Debug.WriteLine(str);
            act.Invoke();
        }
        return n;
    }
}

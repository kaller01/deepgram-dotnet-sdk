﻿// Copyright 2021-2024 Deepgram .NET SDK contributors. All Rights Reserved.
// Use of this source code is governed by a MIT license that can be found in the LICENSE file.
// SPDX-License-Identifier: MIT

using Deepgram.Models.Authenticate.v1;
using Deepgram.Models.PreRecorded.v1;

namespace Deepgram.Clients.PreRecorded.v1;

/// <summary>
/// Implements version 1 of the Analyze Client.
/// </summary>
/// <param name="apiKey">Required DeepgramApiKey</param>
/// <param name="deepgramClientOptions"><see cref="DeepgramHttpClientOptions"/> for HttpClient Configuration</param>
public class Client(string? apiKey = null, DeepgramHttpClientOptions? deepgramClientOptions = null)
    : AbstractRestClient(apiKey, deepgramClientOptions)

{
    #region NoneCallBacks
    /// <summary>
    ///  Transcribe a file by providing a url 
    /// </summary>
    /// <param name="source">Url to the file that is to be transcribed <see cref="UrlSource"></param>
    /// <param name="prerecordedSchema">Options for the transcription <see cref="PrerecordedSchema"/></param>
    /// <returns><see cref="SyncResponse"/></returns>
    public async Task<SyncResponse> TranscribeUrl(UrlSource source, PrerecordedSchema? prerecordedSchema,
        CancellationTokenSource? cancellationToken = default, Dictionary<string, string>? addons = null, Dictionary<string, string>? headers = null)
    {
        VerifyNoCallBack(nameof(TranscribeUrl), prerecordedSchema);

        return await PostAsync<UrlSource, PrerecordedSchema, SyncResponse>(
            GetUri(_options, $"{UriSegments.LISTEN}"), prerecordedSchema, source, cancellationToken, addons, headers
            );
    }
    /// <summary>
    /// Transcribes a file using the provided byte array
    /// </summary>
    /// <param name="source">file is the form of a byte[]</param>
    /// <param name="prerecordedSchema">Options for the transcription <see cref="PrerecordedSchema"/></param>
    /// <returns><see cref="SyncResponse"/></returns>
    public async Task<SyncResponse> TranscribeFile(byte[] source, PrerecordedSchema? prerecordedSchema,
        CancellationTokenSource? cancellationToken = default, Dictionary<string, string>? addons = null, Dictionary<string, string>? headers = null)
    {
        MemoryStream stream = new MemoryStream(source);
        return await TranscribeFile(stream, prerecordedSchema, cancellationToken, addons, headers);
    }

    /// <summary>
    /// Transcribes a file using the provided stream
    /// </summary>
    /// <param name="source">file is the form of a streasm <see cref="Stream"/></param>
    /// <param name="prerecordedSchema">Options for the transcription <see cref="PrerecordedSchema"/></param>
    /// <returns><see cref="SyncResponse"/></returns>
    public async Task<SyncResponse> TranscribeFile(Stream source, PrerecordedSchema? prerecordedSchema, CancellationTokenSource? cancellationToken = default, Dictionary<string, string>? addons = null, Dictionary<string, string>? headers = null)
    {
        VerifyNoCallBack(nameof(TranscribeFile), prerecordedSchema);

        return await PostAsync<Stream, PrerecordedSchema, SyncResponse>(
            GetUri(_options, $"{UriSegments.LISTEN}"), prerecordedSchema, source, cancellationToken, addons, headers
            );
    }

    #endregion

    #region  CallBack Methods
    /// <summary>
    /// Transcribes a file using the provided byte array and providing a CallBack
    /// </summary>
    /// <param name="source">file is the form of a byte[]</param>  
    /// <param name="callBack">CallBack url</param>    
    /// <param name="prerecordedSchema">Options for the transcription<see cref="PrerecordedSchema"></param>
    /// <returns><see cref="AsyncResponse"/></returns>
    public async Task<AsyncResponse> TranscribeFileCallBack(byte[] source, string? callBack, PrerecordedSchema? prerecordedSchema, CancellationTokenSource? cancellationToken = default, Dictionary<string, string>? addons = null, Dictionary<string, string>? headers = null)
    {
        MemoryStream stream = new MemoryStream(source);
        return await TranscribeFileCallBack(stream, callBack, prerecordedSchema, cancellationToken, addons, headers);
    }

    /// <summary>
    /// Transcribes a file using the provided stream and providing a CallBack
    /// </summary>
    /// <param name="source">file is the form of a stream <see cref="Stream"></param>  
    /// <param name="callBack">CallBack url</param>    
    /// <param name="prerecordedSchema">Options for the transcription<see cref="PrerecordedSchema"></param>
    /// <returns><see cref="AsyncResponse"/></returns>
    public async Task<AsyncResponse> TranscribeFileCallBack(Stream source, string? callBack, PrerecordedSchema? prerecordedSchema, CancellationTokenSource? cancellationToken = default, Dictionary<string, string>? addons = null, Dictionary<string, string>? headers = null)
    {
        VerifyOneCallBackSet(nameof(TranscribeFileCallBack), callBack, prerecordedSchema);
        if (callBack != null)
            prerecordedSchema.CallBack = callBack;

        return await PostAsync<Stream, PrerecordedSchema, AsyncResponse>(
            GetUri(_options, $"{UriSegments.LISTEN}"), prerecordedSchema, source, cancellationToken, addons, headers
            );
    }

    /// <summary>
    /// Transcribe a file by providing a url and a CallBack
    /// </summary>
    /// <param name="source">Url to the file that is to be transcribed <see cref="UrlSource"/></param>
    /// <param name="callBack">CallBack url</param>    
    /// <param name="prerecordedSchema">Options for the transcription<see cref="PrerecordedSchema"></param>
    /// <returns><see cref="AsyncResponse"/></returns>
    public async Task<AsyncResponse> TranscribeUrlCallBack(UrlSource source, string? callBack, PrerecordedSchema? prerecordedSchema, CancellationTokenSource? cancellationToken = default, Dictionary<string, string>? addons = null, Dictionary<string, string>? headers = null)
    {
        VerifyOneCallBackSet(nameof(TranscribeUrlCallBack), callBack, prerecordedSchema);

        if (callBack != null)
            prerecordedSchema.CallBack = callBack;

        return await PostAsync<UrlSource, PrerecordedSchema, AsyncResponse>(
            GetUri(_options, $"{UriSegments.LISTEN}"), prerecordedSchema, source, cancellationToken, addons, headers
            );
    }
    #endregion

    #region CallbackChecks
    private void VerifyNoCallBack(string method, PrerecordedSchema? prerecordedSchema)
    {
        if (prerecordedSchema != null && prerecordedSchema.CallBack != null)
            throw new ArgumentException($"CallBack cannot be provided as schema option to a synchronous transcription when calling {method}. Use {nameof(TranscribeFileCallBack)} or {nameof(TranscribeUrlCallBack)}");
    }

    private void VerifyOneCallBackSet(string callingMethod, string? callBack, PrerecordedSchema? prerecordedSchema)
    {

        if (prerecordedSchema.CallBack == null && callBack == null)
        { //check if no CallBack set in either callBack parameter or PrerecordedSchema
            var ex = new ArgumentException($"Either provide a CallBack url or set PrerecordedSchema.CallBack.  If no CallBack needed either {nameof(TranscribeUrl)} or {nameof(TranscribeFile)}");
            Log.Exception(_logger, $"While calling {callingMethod} no callback set", ex);
            throw ex;
        }
        else if (!string.IsNullOrEmpty(prerecordedSchema.CallBack) && !string.IsNullOrEmpty(callBack))
        {
            //check that only one CallBack is set in either callBack parameter or PrerecordedSchema
            var ex = new ArgumentException("CallBack should be set in either the CallBack parameter or PrerecordedSchema.CallBack not in both.");
            Log.Exception(_logger, $"While calling {callingMethod}, callback set in both parameter and property", ex);
            throw ex;
        }
    }
    #endregion    
}

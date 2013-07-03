﻿using System;
using System.Runtime.Serialization;

namespace ArcGIS.ServiceModel.Operation
{
    public static class Operations
    {
        public const String Query = "query";
        public const String ApplyEdits = "applyEdits";
        public const String SingleInputGeocode = "find";
        public const String ReverseGeocode = "reverseGeocode";
        public const String GenerateToken = "generateToken";
    }

    /// <summary>
    /// There are four parameters common to all API operations unless otherwise noted.
    /// </summary>
    [DataContract]
    public abstract class CommonParameters
    {
        protected CommonParameters()
            : this("json", null)
        { }

        protected CommonParameters(Token token)
            : this("json", token)
        { }

        protected CommonParameters(String format, Token token)
        {
            Format = format;
            if (token != null) Token = token.Value;
        }

        /// <summary>
        /// The output format can either be html, json, or pjson
        /// </summary>
        [DataMember(Name = "f")]
        public String Format { get; set; }

        /// <summary>
        /// Generated by the generateToken call, an access token that identifies the authenticated user and controls access to restricted resources and operations.
        /// </summary>
        [DataMember(Name = "token")]
        public String Token { get; set; }

        /// <summary>
        /// Callback is used for JavaScript clients who need a response.
        /// </summary>
        [DataMember(Name = "callback")]
        public String Callback { get; set; }

        /// <summary>
        /// Callback.html wraps the response in html tags for the JavaScript client.
        /// </summary>
        [DataMember(Name = "callback.html")]
        public String CallbackHtml { get; set; }
    }
}

using System.Collections.Generic;
using System;
using System.Linq;
using ArcGIS.ServiceModel.Common;
using System.Runtime.Serialization;

namespace ArcGIS.ServiceModel.Operation
{
    #region QueryAttachments

    /// <summary>
    /// AttachmentsQuery request operation
    /// </summary>
    [DataContract]
    public class AttachmentsQuery : ArcGISServerOperation
    {
        private readonly ArcGISServerEndpoint _endpoint;
        private long _objectId;

        /// <summary>
        /// Represents a request for a querying attachments against a service resource
        /// </summary>
        /// <param name="endpoint">Resource to apply the query against</param>
        public AttachmentsQuery(ArcGISServerEndpoint endpoint)
        {
            _endpoint = endpoint;
            Guard.AgainstNullArgument("endpoint", endpoint);
        }

        public long FeatureObjectId
        {
            get { return _objectId; }
            set {
                _objectId = value;
                Endpoint = new ArcGISServerEndpoint(_endpoint.RelativeUrl.Trim('/') + "/" + string.Format(Operations.QueryAttachments, value));
            }
        }
    }

    [DataContract]
    public class AttachmentsQueryResponse : PortalResponse
    {
        [DataMember(Name = "attachmentInfos")]
        public AttachmentsQueryResult[] AttachmentInfos { get; set; }
    }

    [DataContract]
    public class AttachmentsQueryResult
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "contentType")]
        public string ContentType { get; set; }

        [DataMember(Name = "size")]
        public int Size { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
        
    }

    #endregion

    #region Get Attachments

    /// <summary>
    /// Get Attachments request operation
    /// </summary>
    [DataContract]
    public class AttachmentsGetFile : ArcGISServerOperation
    {
        private readonly ArcGISServerEndpoint _endpoint;
        private long _featureObjectId;
        private long _attachmentId;

        /// <summary>
        /// Represents a request for a querying attachments against a service resource
        /// </summary>
        /// <param name="endpoint">Resource to apply the query against</param>
        public AttachmentsGetFile(ArcGISServerEndpoint endpoint)
        {
            _endpoint = endpoint;
            Guard.AgainstNullArgument("endpoint", endpoint);
        }

        public long FeatureObjectId
        {
            get { return _featureObjectId; }
            set
            {
                _featureObjectId = value;
                Endpoint = new ArcGISServerEndpoint(_endpoint.RelativeUrl.Trim('/') + "/" + string.Format(Operations.GetAttachment, _featureObjectId, _attachmentId));
            }
        }

        public long AttachmentId
        {
            get { return _attachmentId; }
            set
            {
                _attachmentId = value;
                Endpoint = new ArcGISServerEndpoint(_endpoint.RelativeUrl.Trim('/') + "/" + string.Format(Operations.GetAttachment, _featureObjectId, _attachmentId));
            }
        }
    }

    [DataContract]
    public class AttachmentsGetFileResponse : PortalResponse
    {
        [DataMember(Name = "attachmentInfos")]
        public AttachmentsGetFileResult AttachmentsGetFileResult { get; set; }
    }

    [DataContract]
    public class AttachmentsGetFileResult
    {
        public byte[] File { get; set; }

        public string Name { get; set; }

        public string ContentType { get; set; }

    }

    #endregion

    #region Delete Attachments

    /// <summary>
    /// Delete Attachments request operation
    /// </summary>
    [DataContract]
    public class AttachmentsDelete : ArcGISServerOperation
    {
        private readonly ArcGISServerEndpoint _endpoint;
        private long _featureObjectId;
        

        /// <summary>
        /// Represents a request for a querying attachments against a service resource
        /// </summary>
        /// <param name="endpoint">Resource to apply the query against</param>
        public AttachmentsDelete(ArcGISServerEndpoint endpoint)
        {
            _endpoint = endpoint;
            Guard.AgainstNullArgument("endpoint", endpoint);
        }

        public long FeatureObjectId
        {
            get { return _featureObjectId; }
            set
            {
                _featureObjectId = value;
                Endpoint = new ArcGISServerEndpoint(_endpoint.RelativeUrl.Trim('/') + "/" + string.Format(Operations.DeleteAttachments, _featureObjectId));
            }
        }

        [IgnoreDataMember]
        public List<long> AttachmentIds { get; set; }

        [DataMember(Name = "attachmentIds")]
        public string AttachmentIdsValue => AttachmentIds == null || !AttachmentIds.Any() ? null : string.Join(",", AttachmentIds);
    }

    [DataContract]
    public class AttachmentsDeleteResponse : PortalResponse
    {
        [DataMember(Name = "deleteAttachmentResults")]
        public List<AttachmentsDeleteResult> AttachmentsDeleteResults { get; set; }
    }

    [DataContract]
    public class AttachmentsDeleteResult
    {
        [DataMember(Name = "objectId")]
        public long ObjectId { get; set; }

        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "error")]
        public ArcGISError Error { get; set; }
    }

    #endregion

    #region Add Attachments

    /// <summary>
    /// Add Attachments request operation
    /// </summary>
    public interface IAttachment
    {
        byte[] Attachment { get; }

        string FileName { get; }

        string ContentType { get; }
    }

    public class AttachmentToPost : ArcGISServerOperation, IAttachment
    {
        private long _objectId;

                public AttachmentToPost(ArcGISServerEndpoint endpoint, long objectId, string attachmentBase64Encoded, string fileName, string contentType)
            : this(endpoint, objectId, fileName, contentType)
        {
            Guard.AgainstNullArgument("attachmentBase64Encoded", attachmentBase64Encoded);

            AttachmentBase64Encoded = attachmentBase64Encoded;
            Attachment = Convert.FromBase64String(AttachmentBase64Encoded);
        }

        public AttachmentToPost(ArcGISServerEndpoint endpoint, long objectId, byte[] attachment, string fileName, string contentType)
            : this(endpoint, objectId, fileName, contentType)
        {
            Guard.AgainstNullArgument("attachment", attachment);

            Attachment = attachment;
            AttachmentBase64Encoded = Convert.ToBase64String(Attachment);
        }

        AttachmentToPost(ArcGISServerEndpoint endpoint, long objectId, string fileName, string contentType)
        {
            Endpoint = new ArcGISServerEndpoint(endpoint.RelativeUrl.Trim('/') + "/" + string.Format(Operations.AddAttachment, objectId));
            _objectId = objectId;
            FileName = fileName;
            ContentType = contentType;
        }

        public string AttachmentBase64Encoded { get; private set; }

        public byte[] Attachment { get; private set; }

        public string FileName { get; private set; }

        public string ContentType { get; private set; }
    }

    [DataContract]
    public class AddAttachmentResponse : PortalResponse
    {
        [DataMember(Name = "addAttachmentResult")]
        public PostAttachmentResult AddAttachmentResult { get; set; }
    }

    [DataContract]
    public class PostAttachmentResult : PortalResponse
    {
        [DataMember(Name = "objectId")]
        public long ObjectID { get; set; }

        [DataMember(Name = "globalId")]
        public string GlobalID { get; set; }

        [DataMember(Name = "success")]
        public bool Success { get; set; }
    }

    #endregion
}

export interface Attachment {
  fileName: string;
  fileUrl: string;
  contentType: string;
}

export interface ImageAttachment extends Attachment {
  width?: number;
  height?: number;
}

export interface UploadAttachment extends Attachment {
  imageInfo?: ImageInfo;
}

export interface PendingImage {
  file: File;
  tempUrl: string;
}

export interface ImageInfo {
  width: number;
  height: number;
}

//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------

/* tslint:disable */
/* eslint-disable */
// ReSharper disable InconsistentNaming

export interface BcfProject {
    id?: string | undefined;
    name?: string | undefined;
}

export interface BcfFileAttachment {
    name: string;
    base64Data?: string | undefined;
}

export interface BcfProjectExtensions {
    users: string[];
    topicLabels: string[];
    topicTypes: string[];
    priorities: string[];
    snippetTypes: string[];
    topicStatuses: string[];
}

export interface BcfTopic {
    id: string;
    files: BcfTopicFile[];
    viewpoints: BcfViewpoint[];
    documentReferences: BcfDocumentReference[];
    comments: BcfComment[];
    assignedTo?: string | undefined;
    creationAuthor?: string | undefined;
    creationDate?: Date | undefined;
    description?: string | undefined;
    modifiedAuthor?: string | undefined;
    modifiedDate?: Date | undefined;
    serverAssignedId?: string | undefined;
    topicStatus?: string | undefined;
    title?: string | undefined;
    stage?: string | undefined;
    priority?: string | undefined;
    topicType?: string | undefined;
    dueDate?: Date | undefined;
    index?: number | undefined;
    labels: string[];
    relatedTopicIds: string[];
    referenceLinks: string[];
}

export interface BcfTopicFile {
    fileName: string;
    referenceLink?: string | undefined;
    ifcProjectId?: string | undefined;
    ifcSpatialStructureElementId?: string | undefined;
    date?: Date | undefined;
}

export interface BcfViewpoint {
    id: string;
    clippingPlanes: BcfViewpointClippingPlane[];
    lines: BcfViewpointLine[];
    snapshotBase64?: string | undefined;
    orthogonalCamera?: BcfViewpointOrthogonalCamera | undefined;
    perspectiveCamera?: BcfViewpointPerspectiveCamera | undefined;
    viewpointComponents: BcfViewpointComponents;
}

export interface BcfViewpointClippingPlane {
    location: BcfViewpointPoint;
    direction: BcfViewpointVector;
}

export interface BcfViewpointPoint {
    x: number;
    y: number;
    z: number;
}

export interface BcfViewpointVector {
    x: number;
    y: number;
    z: number;
}

export interface BcfViewpointLine {
    startPoint: BcfViewpointPoint;
    endPoint: BcfViewpointPoint;
}

export interface BcfViewpointCameraBase {
    aspectRatio: number;
    direction: BcfViewpointVector;
    upVector: BcfViewpointVector;
    viewPoint: BcfViewpointPoint;
}

export interface BcfViewpointOrthogonalCamera extends BcfViewpointCameraBase {
    viewToWorldScale: number;
}

export interface BcfViewpointPerspectiveCamera extends BcfViewpointCameraBase {
    fieldOfView: number;
}

export interface BcfViewpointComponents {
    coloring: BcfViewpointComponentColoring[];
    selectedComponents: BcfViewpointComponent[];
    visibility: BcfViewpointComponentVisibility;
}

export interface BcfViewpointComponentColoring {
    color: string;
    components: BcfViewpointComponent[];
}

export interface BcfViewpointComponent {
    originatingSystem?: string | undefined;
    authoringToolId?: string | undefined;
    ifcGuid: string;
}

export interface BcfViewpointComponentVisibility {
    defaultVisibility: boolean;
    exceptions: BcfViewpointComponent[];
}

export interface BcfDocumentReference {
    id: string;
    url?: string | undefined;
    documentId?: string | undefined;
    description?: string | undefined;
}

export interface BcfComment {
    id: string;
    text?: string | undefined;
    author?: string | undefined;
    creationDate?: Date | undefined;
    modifiedBy?: string | undefined;
    modifiedDate?: Date | undefined;
    viewpointId?: string | undefined;
}

export interface BcfFile {
    fileName: string;
    project?: BcfProject | undefined;
    fileAttachments?: BcfFileAttachment[] | undefined;
    projectExtensions?: BcfProjectExtensions | undefined;
    topics: BcfTopic[];
}

export interface FrontendConfig {
    isInElectronMode: boolean;
    isConnectedToRevit: boolean;
}

export interface Settings {
    username: string;
}
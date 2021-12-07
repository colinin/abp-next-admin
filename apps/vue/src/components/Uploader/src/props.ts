import type { PropType } from 'vue';
import { File, Chunk } from './typing';

export const basicProps = {
  target: {
    type: String as PropType<string | Function>,
    default: '/',
  },
  singleFile: {
    type: Boolean as PropType<boolean>,
    default: false,
  },
  chunkSize: {
    type: Number as PropType<number>,
    default: 1 * 1024 * 1024,
  },
  forceChunkSize: {
    type: Boolean as PropType<boolean>,
    default: false,
  },
  simultaneousUploads: {
    type: Number as PropType<number>,
    default: 3,
  },
  fileParameterName: {
    type: String as PropType<string>,
    default: 'file',
  },
  query: {
    type: Object as PropType<object | Function>,
    default: {},
  },
  headers: {
    type: Object as PropType<object | Function>,
    default: {},
  },
  withCredentials: {
    type: Boolean as PropType<boolean>,
    default: false,
  },
  method: {
    type: String as PropType<'multipart' | 'octet'>,
    default: 'multipart',
  },
  testMethod: {
    type: String as PropType<string | Function>,
    default: 'GET',
  },
  uploadMethod: {
    type: String as PropType<string | Function>,
    default: 'POST',
  },
  allowDuplicateUploads: {
    type: Boolean as PropType<boolean>,
    default: false,
  },
  prioritizeFirstAndLastChunk: {
    type: Boolean as PropType<boolean>,
    default: false,
  },
  testChunks: {
    type: Boolean as PropType<boolean>,
    default: true,
  },
  preprocess: {
    type: Function as PropType<Function | null>,
    default: null,
  },
  initFileFn: {
    type: Function as PropType<Function | null>,
    default: null,
  },
  readFileFn: {
    type: Function as PropType<Function | null>,
    default: null,
  },
  checkChunkUploadedByResponse: {
    type: Function as PropType<Function | null>,
    default: null,
  },
  generateUniqueIdentifier: {
    type: Function as PropType<Function | null>,
    default: null,
  },
  maxChunkRetries: {
    type: Number as PropType<number | undefined>,
    default: 0,
  },
  chunkRetryInterval: {
    type: Number as PropType<number | null>,
    default: null,
  },
  progressCallbacksInterval: {
    type: Number as PropType<number>,
    default: 500,
  },
  speedSmoothingFactor: {
    type: Number as PropType<number>,
    default: 0.1,
    validator: (value: number) => {
      return value >= 0 && value <= 1;
    },
  },
  successStatuses: {
    type: Array as PropType<number[]>,
    default: [200, 201, 202],
  },
  permanentErrors: {
    type: Array as PropType<number[]>,
    default: [404, 415, 500, 501],
  },
  initialPaused: {
    type: Boolean as PropType<boolean>,
    default: false,
  },
  processResponse: {
    type: Function as PropType<Function>,
    default: (response, cb, file: File, chunk: Chunk) => {
      cb(null, response, file, chunk);
    },
  },
  processParams: {
    type: Function as PropType<Function>,
    default: (params: any, _file: File, _chunk: Chunk, _isTest: boolean) => {
      return params;
    },
  },
  attributes: {
    type: Object as PropType<object>,
    default: {},
  },
};

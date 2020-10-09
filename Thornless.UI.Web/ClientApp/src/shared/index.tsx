export {default as Shared} from './shared';
export {default as SanitizedHtml} from './ReactSanitizedHtml';

export interface ApiResponse<T> {
    data: T;
};

export enum LoadingStates {
    IsLoading,
    IsLoaded,
    IsNotStarted
};

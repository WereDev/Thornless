export interface ApiResponse<T> {
    data: T;
}

export interface LoadingState {
    loadState: LoadingStates;
}

export enum LoadingStates {
    IsLoading,
    IsLoaded,
    IsNotStarted
};

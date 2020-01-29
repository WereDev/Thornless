export interface ApiResponse<T> {
    data: T;
}

export enum LoadingStates {
    IsLoading,
    IsLoaded,
    IsNotStarted
};

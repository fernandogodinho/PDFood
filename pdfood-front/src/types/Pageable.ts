export type Pageable = {
    content: Product[],
    offset: number;
    limit: number;
    hasNext: boolean
};
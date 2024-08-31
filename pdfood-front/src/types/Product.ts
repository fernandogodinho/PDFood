export type Product = {
    id: number;
    name: string;
    price: number;
    barCode: string;
    Image: Image;
};

export type Image = {
    id: number;
    name: string;
    byte: byte[];
    url: string;
};
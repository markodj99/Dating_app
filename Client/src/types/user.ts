export interface User {
    id: string,
    username: string,
    email: string,
    token: string,
    imageUrl?: string
}

export interface LoginCreds {
    email: string,
    password: string
}

export interface RegisterCreds {
    email: string,
    username: string,
    password: string
}
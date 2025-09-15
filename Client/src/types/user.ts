export interface User {
    id: string,
    userName: string,
    email: string,
    token: string,
    imageUrl?: string,
    roles: string[]
}
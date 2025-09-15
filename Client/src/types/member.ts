export interface Member {
    id:string;
    dateOfBirth: string;
    imageUrl?: string;
    userName: string;
    created: string;
    lastActive: string;
    gender: string;
    description?: string;
    city: string;
    country: string;
}

export class MemberParams {
    gender?: string;
    minAge = 18;
    maxAge = 100;
    pageNumber = 1;
    pageSize = 10;
    orderBy = "lastActive";
}

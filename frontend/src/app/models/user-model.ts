export interface userModel {
    id: number
    paypalClientId: string
    paypalClientSecret: string
    email: string
    firstName: string
    lastName: string
    avatarUrl: string | null
    createdAt: Date
}

export interface userPaypalModel {
    paypalClientId: string
    paypalClientSecret: string
}
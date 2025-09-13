export interface Message {
  id: string,
  content: string,
  dateRead?: string,
  messageSent: string,
  senderId: string,
  senderUsername: string,
  senderImageUrl: string,
  recipientId: string,
  recipientUsername: string,
  recipientImageUrl: string,
  currentUserSender?: boolean
}
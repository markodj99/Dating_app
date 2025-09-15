export interface Message {
  id: string,
  content: string,
  dateRead?: string,
  messageSent: string,
  senderId: string,
  senderUserName: string,
  senderImageUrl: string,
  recipientId: string,
  recipientUserName: string,
  recipientImageUrl: string,
  currentUserSender?: boolean
}
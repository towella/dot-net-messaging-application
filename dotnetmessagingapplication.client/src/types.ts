export interface Chat {
  chatId: number,
  name: string,
  messages: Array<Message>,
}

export interface Message {
  id: number | null,
  authorId: number,
  senderUser: string,
  body: string,
  chatId: number,
  imageUrl?: string | null,
}
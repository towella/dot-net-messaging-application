export interface Chat {
  chatId: number,
  name: string,
  messages: Array<Message>,
}

export interface Message {
  id: number | null,
  authorId: number,
  authorName: string,
  body: string,
  chatId: number,
  imageUrl?: string,
}
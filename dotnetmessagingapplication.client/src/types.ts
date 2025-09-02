export interface Chat {
  id: number,
  name: string,
  messages: Array<Message>,
}

export interface Message {
  authorId: number,
  authorName: string,
  body: string,
  chatId: number,
  imageUrl?: string,
}
export interface Chat {
  id: string,
  name: string,
  messages: Array<Message>,
}

export interface Message {
  authorId: string,
  authorName: string,
  body: string,
  chatId: string,
}
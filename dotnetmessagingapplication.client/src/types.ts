export interface Chat {
  id: string,
  name: string,
  messages: Array<Message>,
}

export interface Message {
  authorId: String,
  authorName: String,
  body: String,
}
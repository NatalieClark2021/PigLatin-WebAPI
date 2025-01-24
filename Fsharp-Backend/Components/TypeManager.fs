module newGame.Components.TypeManager

  type UserSentence = { sentence: string }
  
  type postData = {
      bt: string 
      at: string
  }
  
  type postPacket = {
      dataOne: postData option
      dataTwo: postData option
      dataThree: postData option
  }
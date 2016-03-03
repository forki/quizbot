﻿namespace QuizBot

module Facilitator = 

  open QuizBot.Core
  open QuizBot.WorldBankQuestions
  open QuizBot.Twitter
  open System

  let sleepTime = TimeSpan(0,0,1)

  let announceWinner (participant:Participant) =
    participant
    |> sprintf "%A has won!"
    |> Twitter.postTweet |> ignore 

  let rec loop (sleepTime:TimeSpan) = async {
    
    let question = guessCapitalOfCountryQuestion()  
    let questionID = Twitter.postTweet question.Question

    do! Async.Sleep (sleepTime.TotalMilliseconds |> int)

    let replies = Twitter.grabReplies questionID

    let winner = 
      replies
      |> Array.map (fun r -> 
        { Participant = Participant r.ScreenName
          Timestamp = r.Timestamp
          Answer = r.Message })
      |> Array.toList
      |> Core.determineWinner question

    match winner with
    | None -> Twitter.postTweet "Nobody won." |> ignore
    | Some(winner) -> 
      announceWinner winner.Participant |> ignore
  
    return! loop(sleepTime)
  }

  loop sleepTime |> Async.Start

type BotService () =

  member this.Start () = 
    printfn "Started"

  member this.Stop () =
    printfn "Stopped"
module Render

open Hopac
open Hopac.Extensions
open Hopac.Infixes

type private cout = System.Console

type private Message = 
  | Clear
  | TextAt of XY * string
  | CharAt of XY * char

let private channel = Mailbox<Message>()

let private worker = 
  job {
    match! Mailbox.take channel with
    | Clear -> 
        cout.Clear()
    | TextAt (xy, t) -> 
        cout.SetCursorPosition xy
        cout.Write t
    | CharAt (xy, t) -> 
        cout.SetCursorPosition xy
        cout.Write t
    }

let initialize = 
    cout.CursorVisible <- false
    Job.foreverServer worker |> start 

let private send (msg: Message)   = msg |> Mailbox.send channel |> start

let clear ()      = send <| Clear 
let out  xy text  = send <| TextAt (xy, text)
let outc charAt   = send <| CharAt charAt


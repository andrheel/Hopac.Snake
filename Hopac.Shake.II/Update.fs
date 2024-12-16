module Update

open Hopac

let private loop =  
  job {
    match! Mailbox.take events with
    | Skip              -> ()
    | GrowingFood       -> Game.``plant food`` () 
    | Tick              -> Game.``on tick`` ()
    | Direction newdir  -> Game.``change dir`` newdir 
    | State gamestate   -> 
        Game.state <- gamestate
        match gamestate with
        | Init      -> Game.reset()
        | GameOver  -> Game.``show game over``()
        | Quit      -> start <| IVar.fill exitcode 0
        | _         -> ()
  }

let initialize = Job.foreverServer loop |> start 

module Ticks

open Hopac

let private loop = 
  let fps = 10 
  job {
    let! _ = timeOutMillis (1000 / fps)
    if Game.state.IsPlaying then
        event Tick 
  }

let initialize = Job.foreverServer loop |> start 
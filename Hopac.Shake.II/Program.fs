open Hopac

[<EntryPoint>]
let main _ = 
    Render.initialize 
    Keyboard.initialize
    Ticks.initialize
    Update.initialize
    event <| State Init
    IVar.read exitcode |> run
module Game

open Hopac
open System.Collections.Generic
open System.Linq

let w, h = 80, 20

let mutable state = Init

let private zero() = 
  let border = 
      [ for i in 1 .. w do (i, 1); (i, h)
        for i in 1 .. h do (1, i); (w, i) ]
      |> set
      |> Seq.map (fun e -> KeyValuePair (e, Block.Fence)) 
      |> Dictionary<XY, char>
  {|
    snake   = ResizeArray<XY> [ w / 2, h - 5 ]
    fat     = 5
    dir     = 0, -1
    nextDir = 0, -1
    obstacles = border 
    |}

let mutable private game = zero()



let ``at`` (xy : XY) = 
    match game.snake |> Seq.tryFind ((=)xy) with
    | Some _ -> Block.Body
    | _ ->
    match game.obstacles.TryGetValue xy with
    | true, ch -> ch
    | _ -> Block.Space


let rec ``plant food`` () =
    let point = rnd.Next w + 1, rnd.Next h + 1    
    if ``at`` point <> Block.Space then ``plant food`` () else

    let item = point, Block.Food
    item |> game.obstacles.Add 
    item |> Render.outc   


let ``change dir`` newdir = 
    if game.dir ++ newdir = (0,0) then () else
    game <- {| game with nextDir = newdir |}
    

let ``move snake`` new_head = 
    game.snake.Insert (0, new_head)
    if game.fat <= 0 then
        let tail = game.snake.Last()
        game.snake.RemoveAt (game.snake.Count - 1)
        Render.outc (tail, Block.Space)
    else 
        game <- {| game with fat = game.fat - 1 |}
        let text = sprintf "Snake length : %i" game.snake.Count
        Render.out (w - text.Length, 0) text


let ``on tick`` () = 
    let old_head = game.snake.First()
    let new_head = old_head ++ game.nextDir
    Render.outc (old_head, Block.Body) 
    Render.outc (new_head, Block.Head) 
    match ``at`` new_head with
    | Block.Food -> 
        game <- {| game with dir = game.nextDir; fat = game.fat + 5 |}
        event GrowingFood
        ``move snake`` new_head
    | Block.Space -> 
        game <- {| game with dir = game.nextDir |}
        ``move snake`` new_head
    | _ ->     
        event <| State GameOver
        

let ``show game over``() =
    Render.out (w / 2 - 20, h / 2 )     "           G A M E    O V E R           "
    Render.out (w / 2 - 20, h / 2 + 1 ) "          Press 'r' to restart          "


let reset()   = 
    game <- zero() 
    Render.clear()
    for KeyValue o in game.obstacles do Render.outc o
    for n in 1..5 do ``plant food``()
    event <| State Playing

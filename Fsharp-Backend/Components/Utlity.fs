namespace newGame.Components.Utlity

module PLTranslator =
    let vowels = Set  ['a'; 'e'; 'i'; 'o'; 'u'] 

    let transform (input: string) = 
        input.ToLower()
        |> Seq.tryFindIndex (fun vowel -> Set.contains vowel vowels) |> Option.defaultValue 0 

    let findQ  (input: string)  =
        let lowerInput = input.ToLower()
        let quIndex = lowerInput.IndexOf("qu")
        quIndex
        
    let findYAtConsonantClusterEnd (word: string) : int=
        let isConsonant c = not (vowels.Contains c) && c <> 'y'

        let rec findY index =
            if index < 1 then -1
            elif word.[index] = 'y' then
                let prevChar = word.[index - 1]
                let nextChar = if index + 1 < word.Length then Some word.[index + 1] else None
                if isConsonant prevChar && (nextChar.IsNone || not (vowels.Contains nextChar.Value))
                then index
                else findY (index - 1)
            else findY (index - 1)

        findY (word.Length - 1)

    let first (input: string) =
        let yindex = findYAtConsonantClusterEnd input
        let yFH = input.[..yindex - 1] |> String.filter (fun c -> c <> 'y')
        
        let yrest = input.[yindex..]
        let index = transform input
        let quin = findQ input
        let firstHalf = input.[..index-1]
        let firstHalfQ = input.[..quin+1]
        let rest = input.[index..] // Get from the vowel index onward
        let restQ = input.[quin+2..]

        match firstHalf,index, quin,yindex with
        | firstHalf,_,_,_ when ( input.[0] = 'x' && input.[1] = 'r' ) ||( input.[0] = 'y' && input.[1] = 't' ) || ( input.[0] = 'a' && input.[1] = 'y' )->
            input + "ay"
        | _,index,_,yindex when yindex >= 0 && index = 0 ->
            yrest + yFH + "ay"
        | _,_, quin,_ when quin >= 0 && quin < index  ->
            restQ + firstHalfQ + "ay"// Concatenate rest + first half
        | firstHalf,_,_,_ when System.Char.IsDigit input.[0] -> input
        | _ -> rest + firstHalf + "ay"
        
        
    let public translate (input : string) =
         let newput = input.Trim()
         newput.Split ' ' |> Array.toList |> List.map (fun input -> first input) |> String.concat " "
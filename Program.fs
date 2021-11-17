open BenchmarkDotNet.Running
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Diagnosers
open System.Runtime.CompilerServices


type ClassWrapper<[<Measure>] 'Measure, 'Value>(values: array<'Value>) =

    member _.Values = values

    member this.Item
        with get (idx: int<'Measure>) =
            this.Values.[int idx]

    member this.Length = 
        LanguagePrimitives.Int32WithMeasure<'Measure> this.Values.Length


[<Struct>]
type StructWrapper<[<Measure>] 'Measure, 'Value> =
    val Values : array<'Value>

    new (values: array<'Value>) =
        {
            Values = values
        }

    member inline this.Item
        with inline get (idx: int<'Measure>) =
            this.Values.[int idx]

    member this.Length = 
        LanguagePrimitives.Int32WithMeasure<'Measure> this.Values.Length

type RecordWrapper<[<Measure>] 'Measure, 'Value> =
    {
        Values : array<'Value>
    }

    member inline this.Item
        with inline get (idx: int<'Measure>) =
            this.Values.[int idx]

    member this.Length = 
        LanguagePrimitives.Int32WithMeasure<'Measure> this.Values.Length


[<Struct>]
type StructRecordWrapper<[<Measure>] 'Measure, 'Value> =
    {
        Values : array<'Value>
    }

    member inline this.Item
        with inline get (idx: int<'Measure>) =
            this.Values.[int idx]

    member this.Length = 
        LanguagePrimitives.Int32WithMeasure<'Measure> this.Values.Length

module RecordWrapper =

    let create<[<Measure>] 'Measure, 'Value> (values: array<'Value>) =
        {
            Values = values
        } : RecordWrapper<'Measure, 'Value>

[<Measure>] type ItemIdx


let iterations = 100_000
let numberCount = 100

let externalRawArray = [|1.0 .. (float numberCount)|]

let externalClassWrapper =
   [|1.0 .. (float numberCount)|] 
   |> ClassWrapper<ItemIdx, float>

let externalStructWrapper =
    [|1.0 .. (float numberCount)|] 
    |> StructWrapper<ItemIdx, _>

let externalRecordWrapper =
    [|1.0 .. (float numberCount)|] 
    |> RecordWrapper.create<ItemIdx, _>

let externalStructRecordWrapper =
    [|1.0 .. (float numberCount)|] 
    |> RecordWrapper.create<ItemIdx, _>



// [<DisassemblyDiagnoser(maxDepth= 3); DryJob>]
type Benchmarks () =

    let internalRawArray = [|1.0 .. (float numberCount)|]

    let internalClassWrapper =
       [|1.0 .. (float numberCount)|] 
       |> ClassWrapper<ItemIdx, float>

    let internalStructWrapper =
        [|1.0 .. (float numberCount)|] 
        |> StructWrapper<ItemIdx, _>

    let internalRecordWrapper =
        [|1.0 .. (float numberCount)|] 
        |> RecordWrapper.create<ItemIdx, _>

    let internalStructRecordWrapper =
        [|1.0 .. (float numberCount)|] 
        |> RecordWrapper.create<ItemIdx, _>


    [<Benchmark>]
    member _.InternalRawArray () =
        let mutable iterationIdx = 0
        let mutable result = 0.0

        while iterationIdx < iterations do
            let mutable idx = 0
            let len = internalRawArray.Length
            while idx < len do
                result <- result + internalRawArray.[idx]
                idx <- idx + 1
                
            result <- 0.0 // Reset
            iterationIdx <- iterationIdx + 1

        result

    [<Benchmark>]
    member _.ExternalRawArray () =
        let mutable iterationIdx = 0
        let mutable result = 0.0

        while iterationIdx < iterations do
            let mutable idx = 0
            let len = externalRawArray.Length
            while idx < len do
                result <- result + externalRawArray.[idx]
                idx <- idx + 1
                
            result <- 0.0 // Reset
            iterationIdx <- iterationIdx + 1

        result


    [<Benchmark>]
    member _.InternalClassWrapper () =
        let mutable iterationIdx = 0
        let mutable result = 0.0

        while iterationIdx < iterations do
            let mutable idx = 0<ItemIdx>
            let len = internalClassWrapper.Length
            while idx < len do
                result <- result + internalClassWrapper.[idx]
                idx <- idx + 1<ItemIdx>
                
            result <- 0.0 // Reset
            iterationIdx <- iterationIdx + 1

        result

    [<Benchmark>]
    member _.ExternalClassWrapper () =
        let mutable iterationIdx = 0
        let mutable result = 0.0

        while iterationIdx < iterations do
            let mutable idx = 0<ItemIdx>
            let len = externalClassWrapper.Length
            while idx < len do
                result <- result + externalClassWrapper.[idx]
                idx <- idx + 1<ItemIdx>
                
            result <- 0.0 // Reset
            iterationIdx <- iterationIdx + 1

        result
        

    [<Benchmark>]
    member _.InternalStructWrapper () =
        let mutable iterationIdx = 0
        let mutable result = 0.0

        while iterationIdx < iterations do
            let mutable idx = 0<ItemIdx>
            let len = internalStructWrapper.Length
            while idx < len do
                result <- result + internalStructWrapper.[idx]
                idx <- idx + 1<ItemIdx>
                
            result <- 0.0 // Reset
            iterationIdx <- iterationIdx + 1

        result

    [<Benchmark>]
    member _.ExternalStructWrapper () =
        let mutable iterationIdx = 0
        let mutable result = 0.0

        while iterationIdx < iterations do
            let mutable idx = 0<ItemIdx>
            let len = externalStructWrapper.Length
            while idx < len do
                result <- result + externalStructWrapper.[idx]
                idx <- idx + 1<ItemIdx>
                
            result <- 0.0 // Reset
            iterationIdx <- iterationIdx + 1

        result


    [<Benchmark>]
    member _.InternalRecordApproach () =
        let mutable iterationIdx = 0
        let mutable result = 0.0

        while iterationIdx < iterations do
            let mutable idx = 0<ItemIdx>
            let len = internalRecordWrapper.Length
            while idx < len do
                result <- result + internalRecordWrapper.[idx]
                idx <- idx + 1<ItemIdx>
                
            result <- 0.0 // Reset
            iterationIdx <- iterationIdx + 1

        result

    [<Benchmark>]
    member _.ExternalRecordApproach () =
        let mutable iterationIdx = 0
        let mutable result = 0.0

        while iterationIdx < iterations do
            let mutable idx = 0<ItemIdx>
            let len = externalRecordWrapper.Length
            while idx < len do
                result <- result + externalRecordWrapper.[idx]
                idx <- idx + 1<ItemIdx>
                
            result <- 0.0 // Reset
            iterationIdx <- iterationIdx + 1

        result


    [<Benchmark>]
    member _.InternalStructRecordApproach () =
        let mutable iterationIdx = 0
        let mutable result = 0.0

        while iterationIdx < iterations do
            let mutable idx = 0<ItemIdx>
            let len = internalStructRecordWrapper.Length
            while idx < len do
                result <- result + internalStructRecordWrapper.[idx]
                idx <- idx + 1<ItemIdx>
                
            result <- 0.0 // Reset
            iterationIdx <- iterationIdx + 1

        result


    [<Benchmark>]
    member _.ExternalStructRecordApproach () =
        let mutable iterationIdx = 0
        let mutable result = 0.0

        while iterationIdx < iterations do
            let mutable idx = 0<ItemIdx>
            let len = internalStructRecordWrapper.Length
            while idx < len do
                result <- result + internalStructRecordWrapper.[idx]
                idx <- idx + 1<ItemIdx>
                
            result <- 0.0 // Reset
            iterationIdx <- iterationIdx + 1

        result


[<EntryPoint>]
let main (argv: array<string>) =

    let summary = BenchmarkRunner.Run<Benchmarks>()

    0
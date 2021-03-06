﻿namespace QuizBot

module StarWarsQuestions =

  open System
  open StarWars.API
  open QuizBot.Core

  let movies = getAllFilms()

  let planets =
    getAllPlanets()
    |> Array.map(fun m -> m.Name)
    |> Set.ofArray

  let planetPopulation =
    getAllPlanets()
    |> Array.map(fun m -> m.Name, m.Population.Number)
    |> Array.where(fun (name, population) -> population <> None)
  
  let planetPopulationCount = planetPopulation.Length
  
  let rng = Random()

  let getRandomPlanetPopulation () =
    let i = rng.Next(planetPopulationCount)
    planetPopulation.[i]

  let guessThreePlanetsQuestion () =
    { Question = sprintf "Name three planets from Star Wars. [1,2,3]"
      ExpectedAnswer = Category.XOf (3, planets) }

  let guessPlanetPopulation () =
    let planet, population = getRandomPlanetPopulation ()
    { Question = sprintf "What is the population of the planet %s in Star Wars?" planet
      ExpectedAnswer = Category.Closest (float population.Value) }
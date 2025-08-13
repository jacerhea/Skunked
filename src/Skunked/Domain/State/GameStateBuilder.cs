using Skunked.Cards;
using Skunked.Cards.Order;
using Skunked.Cards.Value;
using Skunked.Domain.Events;
using Skunked.Players;
using Skunked.Score;
using Skunked.Utility;

namespace Skunked.Domain.State;

/// <summary>
/// Builds the GameState from the game events.
/// </summary>
public sealed class GameStateBuilder : IEventListener
{
    private readonly GameState _gameState;
    private readonly ScoreCalculator _scoreCalculator;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameStateBuilder"/> class.
    /// </summary>
    public GameStateBuilder(GameState gameState)
    {
        _gameState = gameState;
        _scoreCalculator = new ScoreCalculator();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="event"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void Notify(StreamEvent @event)
    {
        switch (@event)
        {
            case GameStartedEvent t1: Handle(t1, _gameState); break;
            case DeckShuffledEvent t1: Handle(t1, _gameState); break;
            case CardCutEvent t1: Handle(t1, _gameState); break;
            case PlayStartedEvent t1: Handle(t1, _gameState); break;
            case RoundStartedEvent t1: Handle(t1, _gameState); break;
            case HandsDealtEvent t1: Handle(t1, _gameState); break;
            case CardsThrownEvent t1: Handle(t1, _gameState); break;
            case PlayFinishedEvent t1: Handle(t1, _gameState); break;
            case StarterCardSelectedEvent t1: Handle(t1, _gameState); break;
            case CardPlayedEvent t1: Handle(t1, _gameState); break;
            case HandCountedEvent t1: Handle(t1, _gameState); break;
            case CribCountedEvent t1: Handle(t1, _gameState); break;

            default:
                throw new NotImplementedException($"Event Type {@event.GetType()} not implemented.");
        }
    }

    /// <summary>
    /// Handle cref="GameStartedEvent" event.
    /// </summary>
    /// <param name="streamEvent">Event when the game has started.</param>
    /// <param name="gameState">The game state.</param>
    private void Handle(GameStartedEvent streamEvent, GameState gameState)
    {
        var deck = new Deck().ToList();
        gameState.Id = streamEvent.GameId;
        gameState.GameRules = streamEvent.Rules;
        gameState.StartedAt = streamEvent.Occurred;
        gameState.LastUpdated = streamEvent.Occurred;
        gameState.OpeningRound = new OpeningRound
        {
            Deck = deck,
            Complete = false,
            CutCards = new List<PlayerIdCard>(),
            WinningPlayerCut = null,
        };
        gameState.IndividualScores =
            new List<PlayerScore>(streamEvent.Players.Select(player => new PlayerScore { Player = player, Score = 0 }));
        gameState.PlayerIds = streamEvent.Players.ToList();
        gameState.TeamScores = streamEvent.Players.Count == 2
            ? streamEvent.Players.Select(p => new TeamScore { Players = new List<int> { p } }).ToList()
            : new List<TeamScore>
            {
                new() { Players = new List<int> { streamEvent.Players[0], streamEvent.Players[2] } },
                new() { Players = new List<int> { streamEvent.Players[1], streamEvent.Players[3] } },
            };
        gameState.Rounds = new List<RoundState>();
    }

    /// <summary>
    /// Handle cref="DeckShuffledEvent" event.
    /// </summary>
    /// <param name="streamEvent">Event when the deck has been shuffled.</param>
    /// <param name="gameState">The game state.</param>
    private void Handle(DeckShuffledEvent streamEvent, GameState gameState)
    {
        if (!gameState.OpeningRound.Complete)
        {
            gameState.OpeningRound.Deck = streamEvent.Deck;
        }
        else
        {
            var currentRound = gameState.GetCurrentRound();
            currentRound.PreRound ??= new();
            currentRound.PreRound.Deck = streamEvent.Deck;
        }
    }

    /// <summary>
    /// Handle cref="CardCutEvent" event.
    /// </summary>
    /// <param name="streamEvent">Event when a card has been cut.</param>
    /// <param name="gameState">The game state.</param>
    private void Handle(CardCutEvent streamEvent, GameState gameState)
    {
        gameState.OpeningRound.CutCards.Add(new PlayerIdCard
            { Player = streamEvent.PlayerId, Card = new Card(streamEvent.CutCard) });

        bool isDone = gameState.PlayerIds.Count == gameState.OpeningRound.CutCards.Count;
        gameState.OpeningRound.Complete = isDone;

        if (isDone && gameState.Rounds.Count == 0)
        {
            var winningPlayerCut =
                gameState.OpeningRound.CutCards.MinBy(playerCard => playerCard.Card, RankComparer.Instance)!;
            gameState.OpeningRound.WinningPlayerCut = winningPlayerCut.Player;
        }
    }

    /// <summary>
    /// Handle cref="PlayStartedEvent" event.
    /// </summary>
    /// <param name="streamEvent">Event when play has started.</param>
    /// <param name="gameState">The game state.</param>
    private void Handle(PlayStartedEvent streamEvent, GameState gameState)
    {
    }

    /// <summary>
    /// Handle cref="RoundStartedEvent" event.
    /// </summary>
    /// <param name="streamEvent">Event to handle.</param>
    /// <param name="gameState">The game state.</param>
    private void Handle(RoundStartedEvent streamEvent, GameState gameState)
    {
        var playerShowScores = new List<PlayerScoreShow>(gameState.PlayerIds.Select(player => new PlayerScoreShow
            { CribScore = null, HasShowed = false, Player = player, PlayerCountedShowScore = 0, ShowScore = 0 }));

        var currentRound = gameState.Rounds.Count == 0 ? 0 : gameState.GetCurrentRound().Round;

        int cribPlayerId;
        if (gameState.OpeningRound.Complete && gameState.Rounds.Count != 0)
        {
            cribPlayerId = gameState.PlayerIds.NextOf(gameState.PlayerIds.Single(sp =>
                gameState.Rounds.Single(r => r.Round == currentRound).PlayerCrib == sp));
        }
        else
        {
            cribPlayerId = gameState.OpeningRound.WinningPlayerCut.Value;
        }

        var roundState = new RoundState
        {
            Crib = new List<Card>(),
            DealtCards = new List<PlayerHand>(),
            Complete = false,
            PlayerCrib = cribPlayerId,
            Hands = new List<PlayerHand>(),
            ThePlay = new List<List<PlayItem>> { new() },
            Round = currentRound + 1,
            ShowScores = playerShowScores,
        };

        gameState.Rounds.Add(roundState);
    }

    /// <summary>
    /// Handle cref="HandsDealtEvent" event.
    /// </summary>
    /// <param name="streamEvent">Event to handle.</param>
    /// <param name="gameState">The game state.</param>
    private void Handle(HandsDealtEvent streamEvent, GameState gameState)
    {
        var round = gameState.GetCurrentRound();
        round.DealtCards = streamEvent.Hands;
    }

    /// <summary>
    /// Handle cref="CardsThrownEvent" event.
    /// </summary>
    /// <param name="streamEvent">Event to handle.</param>
    /// <param name="gameState">The game state.</param>
    private void Handle(CardsThrownEvent streamEvent, GameState gameState)
    {
        var currentRound = gameState.GetCurrentRound();

        // remove thrown cards from hand
        var playerId = streamEvent.PlayerId;
        var playerHand = currentRound.DealtCards.Single(ph => ph.PlayerId == playerId).Hand.Except(streamEvent.Thrown);
        currentRound.Hands.Add(new PlayerHand(playerId, playerHand.ToList()));

        currentRound.Crib.AddRange(streamEvent.Thrown);

        var playersDoneThrowing = gameState.GetCurrentRound().Crib.Count == GameRules.HandSize;
        currentRound.ThrowCardsComplete = playersDoneThrowing;
    }

    /// <summary>
    /// Handle cref="StarterCardSelectedEvent" event.
    /// </summary>
    /// <param name="streamEvent">Event to handle.</param>
    /// <param name="gameState">The game state.</param>
    private void Handle(StarterCardSelectedEvent streamEvent, GameState gameState)
    {
        var currentRound = gameState.GetCurrentRound();
        var dealerId = currentRound.PlayerCrib;
        var playerScore = gameState.IndividualScores.Single(ps => ps.Player == dealerId);
        var team = gameState.TeamScores.Single(t => t.Players.Contains(dealerId));
        var cutScore = _scoreCalculator.CountCut(streamEvent.Starter);
        playerScore.Score += cutScore;
        team.Score += cutScore;
        currentRound.Starter = streamEvent.Starter;
    }

    /// <summary>
    /// Handle cref="CardPlayedEvent" event.
    /// </summary>
    /// <param name="streamEvent">Event to handle.</param>
    /// <param name="gameState">The game state.</param>
    private void Handle(CardPlayedEvent streamEvent, GameState gameState)
    {
        // 2. Declare round variables
        var currentRound = gameState.GetCurrentRound();
        var setOfPlays = currentRound.ThePlay;

        // 3.  Set new Round before play, if necessary
        var currentPlayRound = setOfPlays.Last();
        // int playCount = _args.ScoreCalculator.SumValues(currentPlayRound.Select(scs => scs.Card).Append(_args.PlayedCard));
        // if (playCount > GameState.Rules.MaxPlayCount)
        // {
        //    var playerPlayItems = new List<PlayerPlayItem>();
        //    setOfPlays.Add(playerPlayItems);
        //    currentPlayRound = playerPlayItems;
        // }

        // 4.  Card is played
        var playerCardPlayedScore = new PlayItem
        {
            Card = new Card(streamEvent.Played),
            Player = streamEvent.PlayerId,
        };
        currentPlayRound.Add(playerCardPlayedScore);
        var playScore = _scoreCalculator.CountPlayPoints(currentPlayRound.Select(psc => psc.Card).ToList());
        // currentPlayerScore.Score += playScore;

        // create new round
        var playedCards = setOfPlays.SelectMany(c => c).Select(spc => spc.Card);
        var playsLeft = gameState.GetCurrentRound().Hands.SelectMany(kv => kv.Hand).Except(playedCards);
        if (playsLeft.All(c =>
                _scoreCalculator.SumValues(currentPlayRound.Select(spc => spc.Card).Append(c)) >
                GameRules.Points.MaxPlayCount))
        {
            // add Go Value.  not counted if 31 as was included with ScoreCalculation.CountThePlay
            int playCountNew = _scoreCalculator.SumValues(currentPlayRound.Select(ppi => ppi.Card));
            playerCardPlayedScore.NewCount = playCountNew;
            if (playCountNew != GameRules.Points.MaxPlayCount)
            {
                var goValue = GameRules.Points.Go;
                playScore += goValue;
            }

            // not done playing, so add new play round
            setOfPlays.Add(new List<PlayItem>());
        }

        var currentPlayerScore = gameState.IndividualScores.Single(ps => ps.Player == streamEvent.PlayerId);
        var currentTeamScore = gameState.TeamScores.Single(ps => ps.Players.Contains(streamEvent.PlayerId));
        playerCardPlayedScore.NextPlayer = FindNextPlayer(gameState, streamEvent.PlayerId);
        playerCardPlayedScore.Score += playScore;
        currentPlayerScore.Score += playScore;
        currentTeamScore.Score += playScore;

        // 5.  Check if done with Play
        bool isDone = setOfPlays.SelectMany(c => c).Select(spc => spc.Card).Count() ==
                      gameState.PlayerIds.Count * GameRules.HandSize;
        currentRound.PlayedCardsComplete = isDone;
    }

    /// <summary>
    /// Handle cref="HandCountedEvent" event.
    /// </summary>
    /// <param name="streamEvent">Event to handle.</param>
    /// <param name="gameState">The game state.</param>
    private void Handle(HandCountedEvent streamEvent, GameState gameState)
    {
        var roundState = gameState.GetCurrentRound();
        var starterCard = roundState.Starter;
        var playerHand = roundState.Hands.First(ph => ph.PlayerId == streamEvent.PlayerId);

        var calculatedShowScore = _scoreCalculator.CountShowPoints(starterCard, playerHand.Hand);

        var playerScore = gameState.IndividualScores.Single(ps => ps.Player == streamEvent.PlayerId);
        var teamScore = gameState.TeamScores.Single(ps => ps.Players.Contains(streamEvent.PlayerId));
        playerScore.Score += streamEvent.CountedScore;
        teamScore.Score += streamEvent.CountedScore;

        var playerShowScore = gameState.GetCurrentRound().ShowScores.Single(pss => pss.Player == streamEvent.PlayerId);
        playerShowScore.ShowScore = calculatedShowScore.Points.Score;
        playerShowScore.HasShowed = true;
        playerShowScore.Complete = streamEvent.PlayerId != roundState.PlayerCrib;
        playerShowScore.PlayerCountedShowScore = streamEvent.CountedScore;
    }

    /// <summary>
    /// Handle cref="CribCountedEvent" event.
    /// </summary>
    /// <param name="streamEvent">Event to handle.</param>
    /// <param name="gameState">The game state.</param>
    private void Handle(CribCountedEvent streamEvent, GameState gameState)
    {
        var currentRound = gameState.GetCurrentRound();
        var starterCard = currentRound.Starter;
        var crib = currentRound.Crib;

        var calculatedCribShowScore = _scoreCalculator.CountShowPoints(starterCard, crib);

        var calculatedCribScore = calculatedCribShowScore.Points.Score;
        // penalty for over counting
        var applicableScore = 0;
        if (streamEvent.CountedScore == calculatedCribScore)
        {
            applicableScore = calculatedCribScore;
        }
        else if (streamEvent.CountedScore > calculatedCribScore)
        {
            // todo: fix
            // var score = calculatedCribScore - ScorePenalty;
            // applicableScore = score < 0 ? 0 : score;
        }
        else
        {
            applicableScore = streamEvent.CountedScore;
        }

        var playerScore = gameState.IndividualScores.Single(ps => ps.Player == streamEvent.PlayerId);
        var teamScore = gameState.TeamScores.Single(ps => ps.Players.Contains(streamEvent.PlayerId));
        playerScore.Score += applicableScore;
        teamScore.Score += applicableScore;

        var playerShowScore = gameState.GetCurrentRound().ShowScores.Single(pss => pss.Player == streamEvent.PlayerId);
        playerShowScore.CribScore = calculatedCribScore;
        playerShowScore.HasShowedCrib = true;
        playerShowScore.Complete = true;

        currentRound.Complete = true;
    }


    /// <summary>
    /// Handle cref="CribCountedEvent" event.
    /// </summary>
    /// <param name="streamEvent">Event to handle.</param>
    /// <param name="gameState">The game state.</param>
    private void Handle(PlayFinishedEvent streamEvent, GameState gameState)
    {
    }


    private bool CheckEndOfGame(GameState gameState)
    {
        if (gameState.TeamScores.Any(ts => ts.Score >= gameState.GameRules.WinningScore))
        {
            gameState.CompletedAt = DateTimeOffset.Now;
            return true;
        }

        return false;
    }

    private int? FindNextPlayer(GameState state, int playerId)
    {
        var roundState = state.GetCurrentRound();
        var currentRound = roundState;
        var setOfPlays = currentRound.ThePlay;
        var playedCards = setOfPlays.SelectMany(c => c).Select(spc => spc.Card).ToList();
        var playerCardPlayedScores = setOfPlays.Last();

        // if round is done
        if (playedCards.Count == state.PlayerIds.Count * GameRules.HandSize)
        {
            return null;
        }

        // move to current player
        var currentPlayer = state.PlayerIds.Single(sp => sp == playerId);
        var nextPlayer = state.PlayerIds.NextOf(currentPlayer);

        // move to next player with valid move
        while (true)
        {
            var nextPlayerAvailableCardsToPlay = roundState.Hands.Single(ph => ph.PlayerId == nextPlayer).Hand
                .Except(playedCards).ToList();
            if (!nextPlayerAvailableCardsToPlay.Any())
            {
                nextPlayer = state.PlayerIds.NextOf(nextPlayer);
                continue;
            }

            var nextPlayerPlaySequence = playerCardPlayedScores.Select(s => s.Card).ToList();
            nextPlayerPlaySequence.Add(
                nextPlayerAvailableCardsToPlay.MinBy(c => new AceLowFaceTenCardValueStrategy().GetValue(c))!);
            var scoreTest = _scoreCalculator.SumValues(nextPlayerPlaySequence);
            if (scoreTest <= GameRules.Points.MaxPlayCount)
            {
                return nextPlayer;
            }

            nextPlayer = state.PlayerIds.NextOf(nextPlayer);
        }
    }
}
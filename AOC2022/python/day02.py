def solve(data):
    rounds = [x.split(" ") for x in data.splitlines()]
    return solve_part_1(rounds), solve_part_2(rounds)


def score_game(game):
    match game:
        case ("Rock", "Rock"):
            return 1 + 3
        case ("Rock", "Paper"):
            return 2 + 6
        case ("Rock", "Scissors"):
            return 3 + 0
        case ("Paper", "Rock"):
            return 1 + 0
        case ("Paper", "Paper"):
            return 2 + 3
        case ("Paper", "Scissors"):
            return 3 + 6
        case ("Scissors", "Rock"):
            return 1 + 6
        case ("Scissors", "Paper"):
            return 2 + 0
        case ("Scissors", "Scissors"):
            return 3 + 3


def rps_select_score_game(game):
    match game:
        case ("Rock", "X"):
            return 0 + 3
        case ("Rock", "Y"):
            return 3 + 1
        case ("Rock", "Z"):
            return 6 + 2
        case ("Paper", "X"):
            return 0 + 1
        case ("Paper", "Y"):
            return 3 + 2
        case ("Paper", "Z"):
            return 6 + 3
        case ("Scissors", "X"):
            return 0 + 2
        case ("Scissors", "Y"):
            return 3 + 3
        case ("Scissors", "Z"):
            return 6 + 1


def map_to_rps(x, rps_map):
    return rps_map[x]


def solve_part_1(values):
    elf_map = {"A": "Rock", "B": "Paper", "C": "Scissors"}
    rps_map = {"X": "Rock", "Y": "Paper", "Z": "Scissors"}
    games = [(elf_map.get(x), rps_map.get(y)) for x, y in values]
    return sum(map(score_game, games))


def solve_part_2(values):
    elf_map = {"A": "Rock", "B": "Paper", "C": "Scissors"}
    games = [(elf_map.get(x), y) for x, y in values]
    return sum(map(rps_select_score_game, games))


if __name__ == "__main__":
    from AOC2022.python import run_solver

    run_solver(solve, __file__)

from collections import defaultdict


directions = {
    "e": (1, -1, 0),
    "se": (0, -1, 1),
    "sw": (-1, 0, 1),
    "w": (-1, 1, 0),
    "nw": (0, 1, -1),
    "ne": (1, 0, -1),
}


def create_tiles(specs: list):
    tiles = defaultdict(lambda: False)
    for spec in specs:
        tile = (0, 0, 0)
        for instruction in spec:
            tile = tuple(x + d for x, d in zip(tile, directions[instruction]))
        if tile in tiles:
            tiles[tile] = not tiles[tile]
        else:
            tiles[tile] = True
    return tiles


def solve_part_1(specs: list):
    tiles = dict(create_tiles(specs))
    return sum(tiles.values())


def _process_tile(position, tiles):
    n_black_neighbours = 0
    for neighbour in directions.values():
        neighbours_position = (
            position[0] + neighbour[0],
            position[1] + neighbour[1],
            position[2] + neighbour[2],
        )
        if tiles.get(neighbours_position):
            n_black_neighbours += 1
    return n_black_neighbours


def solve_part_2(specs: list):
    tiles = create_tiles(specs)
    for day in range(100):
        next_days_tiles = defaultdict(lambda: False)
        black_tiles = [t[0] for t in tiles.items() if t[1]]
        for bt in black_tiles:
            n = _process_tile(bt, tiles)
            if n == 0 or n > 2:
                next_days_tiles[bt] = False
            else:
                next_days_tiles[bt] = True
            for neighbour in directions.values():
                neighbour_position = (
                    bt[0] + neighbour[0],
                    bt[1] + neighbour[1],
                    bt[2] + neighbour[2],
                )
                if neighbour_position not in next_days_tiles:
                    n = _process_tile(neighbour_position, tiles)
                    if n == 2:
                        next_days_tiles[neighbour_position] = True
                    else:
                        next_days_tiles[neighbour_position] = False
        tiles = next_days_tiles
    return sum(tiles.values())


def solve(data: str):

    tile_specs = []

    for row in data.splitlines():
        parsed_row = []
        this = ""
        for char in row:
            if char in ("n", "s"):
                this += char
            else:
                parsed_row.append(this + char)
                this = ""
        tile_specs.append(parsed_row)

    part_1 = solve_part_1(tile_specs)
    part_2 = solve_part_2(tile_specs)
    return part_1, part_2


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)

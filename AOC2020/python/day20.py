import itertools

opposites = {"right": "left", "left": "right", "up": "down", "down": "up"}
directions = {"right": (1, 0), "left": (-1, 0), "up": (0, 1), "down": (0, -1)}


class Tile:
    def __init__(self, raw_data):
        self.tile_id = int(raw_data[0].strip(":").strip("Tile "))
        self.rows = tuple(raw_data[1:])
        self.columns = []
        for column_id in range(len(self.rows)):
            self.columns.append("".join((row[column_id] for row in self.rows)))
        self.columns = tuple(self.columns)

        self.left = self.columns[0]
        self.right = self.columns[-1]
        self.down = self.rows[0]
        self.up = self.rows[-1]

        self.free_edges = ["left", "right", "up", "down"]

    def rotate_clockwise(self):
        self.rows = self.columns
        self.columns = []
        for column_id in range(len(self.rows)):
            self.columns.append("".join((row[column_id] for row in self.rows)))
        self.columns = tuple(self.columns)
        self.left = self.columns[0]
        self.right = self.columns[-1]
        self.down = self.rows[0]
        self.up = self.rows[-1]

    def flipud(self):
        self.rows = self.rows[::-1]
        self.columns = []
        for column_id in range(len(self.rows)):
            self.columns.append("".join((row[column_id] for row in self.rows)))
        self.columns = tuple(self.columns)
        self.left = self.columns[0]
        self.right = self.columns[-1]
        self.down = self.rows[0]
        self.up = self.rows[-1]

    def fliplr(self):
        self.columns = tuple(self.columns[::-1])
        self.rows = []
        for row_id in range(len(self.columns)):
            self.rows.append("".join((column[row_id] for column in self.columns)))
        self.rows = tuple(self.rows)
        self.left = self.columns[0]
        self.right = self.columns[-1]
        self.down = self.rows[0]
        self.up = self.rows[-1]

    def __repr__(self):
        return f"<Tile {self.tile_id}>"


def place_tiles(tiles):
    placed_tiles = {(0, 0): tiles.pop(0)}

    while tiles:
        print(f"Number of tiles: {len(tiles)}...")
        if len(tiles) == 1:
            # Do not need this one for solution to puzzle... Need to find why it doesn't fit though...
            break
        for tile in tiles:
            if tile in placed_tiles.values():
                tiles.remove(tile)
                break

            has_been_placed = False
            try:
                for placed_tile_pos, placed_tile in placed_tiles.items():
                    if has_been_placed:
                        break
                    for placed_tile_side in placed_tile.free_edges:
                        if has_been_placed:
                            break
                        for _ in range(4):
                            if has_been_placed:
                                break
                            placed_side = getattr(placed_tile, placed_tile_side)
                            opposite_side = getattr(tile, opposites[placed_tile_side])
                            if placed_side == opposite_side:
                                # Possible match. Check all other adjacent tile
                                placable = True
                                proposed_placing = (
                                    placed_tile_pos[0]
                                    + directions[placed_tile_side][0],
                                    placed_tile_pos[1]
                                    + directions[placed_tile_side][1],
                                )
                                for side, direction_tuple in directions.items():
                                    possible_adjacent_tile_pos = (
                                        proposed_placing[0] + direction_tuple[0],
                                        proposed_placing[1] + direction_tuple[1],
                                    )
                                    if possible_adjacent_tile_pos == placed_tile_pos:
                                        continue
                                    else:
                                        adjacent_tile = placed_tiles.get(
                                            possible_adjacent_tile_pos, None
                                        )
                                        if adjacent_tile:
                                            if getattr(
                                                adjacent_tile, opposites[side]
                                            ) == getattr(tile, side):
                                                pass
                                            else:
                                                placable = False
                                                break

                                if placable:
                                    for side, direction_tuple in directions.items():
                                        possible_adjacent_tile_pos = (
                                            proposed_placing[0] + direction_tuple[0],
                                            proposed_placing[1] + direction_tuple[1],
                                        )
                                        adjacent_tile = placed_tiles.get(
                                            possible_adjacent_tile_pos, None
                                        )
                                        if adjacent_tile:
                                            adjacent_tile.free_edges.remove(
                                                opposites[side]
                                            )
                                            tile.free_edges.remove(side)
                                    placed_tiles[proposed_placing] = tile
                                    has_been_placed = True
                                    break
                            else:
                                pass
                            tile.rotate_clockwise()

                        if has_been_placed:
                            break
                        tile.flipud()

                        for _ in range(4):
                            if has_been_placed:
                                break
                            placed_side = getattr(placed_tile, placed_tile_side)
                            opposite_side = getattr(tile, opposites[placed_tile_side])
                            if placed_side == opposite_side:
                                # Possible match. Check all other adjacent tile
                                placable = True
                                proposed_placing = (
                                    placed_tile_pos[0]
                                    + directions[placed_tile_side][0],
                                    placed_tile_pos[1]
                                    + directions[placed_tile_side][1],
                                )
                                for side, direction_tuple in directions.items():
                                    possible_adjacent_tile_pos = (
                                        proposed_placing[0] + direction_tuple[0],
                                        proposed_placing[1] + direction_tuple[1],
                                    )
                                    if possible_adjacent_tile_pos == placed_tile_pos:
                                        continue
                                    else:
                                        adjacent_tile = placed_tiles.get(
                                            possible_adjacent_tile_pos, None
                                        )
                                        if adjacent_tile:
                                            if getattr(
                                                adjacent_tile, opposites[side]
                                            ) == getattr(tile, side):
                                                pass
                                            else:
                                                placable = False
                                                break

                                if placable:
                                    for side, direction_tuple in directions.items():
                                        possible_adjacent_tile_pos = (
                                            proposed_placing[0] + direction_tuple[0],
                                            proposed_placing[1] + direction_tuple[1],
                                        )
                                        adjacent_tile = placed_tiles.get(
                                            possible_adjacent_tile_pos, None
                                        )
                                        if adjacent_tile:
                                            adjacent_tile.free_edges.remove(
                                                opposites[side]
                                            )
                                            tile.free_edges.remove(side)
                                    placed_tiles[proposed_placing] = tile
                                    has_been_placed = True
                                    break
                            else:
                                pass
                            tile.rotate_clockwise()

                        if has_been_placed:
                            break
                        tile.fliplr()

                        for _ in range(4):
                            if has_been_placed:
                                break
                            placed_side = getattr(placed_tile, placed_tile_side)
                            opposite_side = getattr(tile, opposites[placed_tile_side])
                            if placed_side == opposite_side:
                                # Possible match. Check all other adjacent tile
                                placable = True
                                proposed_placing = (
                                    placed_tile_pos[0]
                                    + directions[placed_tile_side][0],
                                    placed_tile_pos[1]
                                    + directions[placed_tile_side][1],
                                )
                                for side, direction_tuple in directions.items():
                                    possible_adjacent_tile_pos = (
                                        proposed_placing[0] + direction_tuple[0],
                                        proposed_placing[1] + direction_tuple[1],
                                    )
                                    if possible_adjacent_tile_pos == placed_tile_pos:
                                        continue
                                    else:
                                        adjacent_tile = placed_tiles.get(
                                            possible_adjacent_tile_pos, None
                                        )
                                        if adjacent_tile:
                                            if getattr(
                                                adjacent_tile, opposites[side]
                                            ) == getattr(tile, side):
                                                pass
                                            else:
                                                placable = False
                                                break

                                if placable:
                                    for side, direction_tuple in directions.items():
                                        possible_adjacent_tile_pos = (
                                            proposed_placing[0] + direction_tuple[0],
                                            proposed_placing[1] + direction_tuple[1],
                                        )
                                        adjacent_tile = placed_tiles.get(
                                            possible_adjacent_tile_pos, None
                                        )
                                        if adjacent_tile:
                                            adjacent_tile.free_edges.remove(
                                                opposites[side]
                                            )
                                            tile.free_edges.remove(side)
                                    placed_tiles[proposed_placing] = tile
                                    has_been_placed = True
                                    break
                            else:
                                pass
                            tile.rotate_clockwise()
            except RuntimeError:
                continue
    return placed_tiles


_data = """Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###

Tile 1951:
#.##...##.
#.####...#
.....#..##
#...######
.##.#....#
.###.#####
###.##.##.
.###....#.
..#.#..#.#
#...##.#..

Tile 1171:
####...##.
#..##.#..#
##.#..#.#.
.###.####.
..###.####
.##....##.
.#...####.
#.##.####.
####..#...
.....##...

Tile 1427:
###.##.#..
.#..#.##..
.#.##.#..#
#.#.#.##.#
....#...##
...##..##.
...#.#####
.#.####.#.
..#..###.#
..##.#..#.

Tile 1489:
##.#.#....
..##...#..
.##..##...
..#...#...
#####...#.
#..#.#.#.#
...#.#.#..
##.#...##.
..##.##.##
###.##.#..

Tile 2473:
#....####.
#..#.##...
#.##..#...
######.#.#
.#...#.#.#
.#########
.###.#..#.
########.#
##...##.#.
..###.#.#.

Tile 2971:
..#.#....#
#...###...
#.#.###...
##.##..#..
.#####..##
.#..####.#
#..#.#..#.
..####.###
..#.#.###.
...#.#.#.#

Tile 2729:
...#.#.#.#
####.#....
..#.#.....
....#..#.#
.##..##.#.
.#.####...
####.#.#..
##.####...
##..#.##..
#.##...##.

Tile 3079:
#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###..."""


def solve(data: str):

    # data = _data
    tiles = []
    tile = []
    for row in data.splitlines():
        if row:
            tile.append(row)
        else:
            tiles.append(Tile(tile))
            tile = []
    tiles.append(Tile(tile))

    placed_tiles = place_tiles(tiles)
    all_xs = [k[0] for k in placed_tiles.keys()]
    all_ys = [k[1] for k in placed_tiles.keys()]
    part_1 = (
        placed_tiles[(max(all_xs), max(all_ys))].tile_id
        * placed_tiles[(min(all_xs), max(all_ys))].tile_id
        * placed_tiles[(max(all_xs), min(all_ys))].tile_id
        * placed_tiles[(min(all_xs), min(all_ys))].tile_id
    )
    part_2 = 0
    return part_1, part_2


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)

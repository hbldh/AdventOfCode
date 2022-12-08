from math import prod


def solve(data):
    trees = [list(map(int, row)) for row in data.splitlines()]
    return solve_part_1(trees), solve_part_2(trees)


def solve_part_1(trees):
    possibles = set()

    for row_idx, row in enumerate(trees[1:-1], start=1):
        max_h = trees[row_idx][0]
        for col_idx_from_left in range(1, len(row) - 1):
            if trees[row_idx][col_idx_from_left] > max_h:
                max_h = trees[row_idx][col_idx_from_left]
                possibles.add((row_idx, col_idx_from_left))
        max_h = trees[row_idx][-1]
        for col_idx_from_right in range(len(row) - 2, 0, -1):
            if trees[row_idx][col_idx_from_right] > max_h:
                max_h = trees[row_idx][col_idx_from_right]
                possibles.add((row_idx, col_idx_from_right))

    trees_transpose = [list(i) for i in zip(*trees)]
    for col_idx, col in enumerate(trees_transpose[1:-1], start=1):
        max_h = trees_transpose[col_idx][0]
        for row_idx_from_top in range(1, len(col) - 1):
            if trees_transpose[col_idx][row_idx_from_top] > max_h:
                max_h = trees_transpose[col_idx][row_idx_from_top]
                possibles.add((row_idx_from_top, col_idx))
        max_h = trees_transpose[col_idx][-1]
        for row_idx_from_bottom in range(len(col) - 2, 0, -1):
            if trees_transpose[col_idx][row_idx_from_bottom] > max_h:
                max_h = trees_transpose[col_idx][row_idx_from_bottom]
                possibles.add((row_idx_from_bottom, col_idx))

    n_visible = len(trees) * 2 + len(trees[0]) * 2 - 4 + len(possibles)
    return n_visible


def calculate_scenic_scores(trees, house_row_idx, house_col_idx):
    scores = [0, 0, 0, 0]
    max_h = trees[house_row_idx][house_col_idx]
    # Look right
    for col_idx_from_left in range(house_col_idx + 1, len(trees[0])):
        scores[0] += 1
        if trees[house_row_idx][col_idx_from_left] >= max_h:
            break
    # Look left
    for col_idx_from_right in range(house_col_idx - 1, -1, -1):
        scores[1] += 1
        if trees[house_row_idx][col_idx_from_right] >= max_h:
            break
    # Look up
    for row_idx_from_top in range(house_row_idx - 1, -1, -1):
        scores[2] += 1
        if trees[row_idx_from_top][house_col_idx] >= max_h:
            break
    # Look down
    for row_idx_from_bottom in range(house_row_idx + 1, len(trees)):
        scores[3] += 1
        if trees[row_idx_from_bottom][house_col_idx] >= max_h:
            break
    return prod(scores)


def solve_part_2(trees):
    scenic_scores = []
    for row_idx in range(1, len(trees) - 1):
        for col_idx in range(1, len(trees[0]) - 1):
            if row_idx == 3 and col_idx == 2:
                print("Now!")
            scenic_scores.append(calculate_scenic_scores(trees, row_idx, col_idx))
    return max(scenic_scores)


if __name__ == "__main__":
    from AOC2022.python import run_solver

    run_solver(solve, __file__)

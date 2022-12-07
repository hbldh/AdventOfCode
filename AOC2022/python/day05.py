from queue import LifoQueue
import textwrap


def parse_stack(data):
    raw_stack = data.split("\n\n")[0]
    rows = [[pos.strip() for pos in textwrap.wrap(row.rstrip(), 4, drop_whitespace=False,)] for row in raw_stack.splitlines()]
    stack = {int(idx): LifoQueue() for idx in rows.pop()}
    for crate_row in rows[::-1]:
        for idx, crate in enumerate(crate_row, start=1):
            if crate:
                stack[idx].put(crate)
    return stack


def parse_moves(data):
    raw_moves = data.split("\n\n")[1]
    moves = [tuple(int(t) for t in move.split(" ")[1::2]) for move in raw_moves.splitlines()]
    return moves


def solve(data):
    return solve_part_1(parse_stack(data), parse_moves(data)), solve_part_2(parse_stack(data), parse_moves(data))


def solve_part_1(stack, moves):
    for n_crates_to_move, from_stack, to_stack in moves:
        for _ in range(n_crates_to_move):
            stack[to_stack].put(stack[from_stack].get_nowait())
    return "".join([stack[x].get().strip('[').strip(']') for x in sorted(stack.keys())])


def solve_part_2(stack, moves):
    for n_crates_to_move, from_stack, to_stack in moves:
        crates = [stack[from_stack].get_nowait() for _ in range(n_crates_to_move)]
        for crate in crates[::-1]:
            stack[to_stack].put(crate)
    return "".join([stack[x].get().strip('[').strip(']') for x in sorted(stack.keys())])


if __name__ == "__main__":
    from AOC2022.python import run_solver
    run_solver(solve, __file__)

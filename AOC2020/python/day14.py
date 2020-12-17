import itertools
import re


def solve_part_1(instructions):
    mem = {}
    current_mask = None
    for instr in instructions:
        if isinstance(instr, list):
            # memory write
            mem[instr[0]] = (instr[1] | current_mask[0]) & current_mask[1]
        else:
            # new mask
            current_mask = (
                int(instr.replace("X", "0"), 2),
                int(instr.replace("X", "1"), 2),
            )
    return sum(mem.values())


def solve_part_2(instructions):
    mem = {}
    current_mask = None
    for instr in instructions:
        if isinstance(instr, list):
            # memory write
            possibilities = []
            b_instr = bin(instr[0])[2:].zfill(36)
            for bit in range(36):
                if current_mask[bit] == "0":
                    possibilities.append(
                        [
                            b_instr[bit],
                        ]
                    )
                elif current_mask[bit] == "1":
                    possibilities.append(
                        [
                            "1",
                        ]
                    )
                else:
                    possibilities.append(["0", "1"])
            for memory_address in itertools.product(*possibilities):
                mem[int("".join(memory_address), 2)] = instr[1]
        else:
            # new mask
            current_mask = instr
    return sum(mem.values())


def solve(data):
    instructions = []
    for row in data.splitlines():
        if row.startswith("mask = "):
            instructions.append(row.strip("mask = "))
        else:
            instructions.append(
                list(map(int, re.search("mem\\[(\\d*)\\] = (\\d*)", row).groups()))
            )

    return solve_part_1(instructions), solve_part_2(instructions)


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)

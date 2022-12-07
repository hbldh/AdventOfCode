

def solve(data):
    return solve_part_1(data), solve_part_2(data)


def solve_part_1(data):
    uniques = [set(row[:len(row)//2]).intersection(set(row[len(row)//2:])).pop() for row in data.splitlines()]
    vals = []
    for char in uniques:
        if char.isupper():
            vals.append(ord(char) - ord('A') + 26 + 1)
        else:
            vals.append(ord(char) - ord('a') + 1)
    return sum(vals)


def solve_part_2(data):
    elves = [row for row in data.splitlines()]
    idx = 0
    badges = []
    while idx < len(elves):
        badges.append(set(elves[idx]).intersection(elves[idx + 1]).intersection(elves[idx + 2]).pop())
        badges.append(set(elves[idx + 3]).intersection(elves[idx + 4]).intersection(elves[idx + 5]).pop())
        idx += 6
    vals = []
    for char in badges:
        if char.isupper():
            vals.append(ord(char) - ord('A') + 26 + 1)
        else:
            vals.append(ord(char) - ord('a') + 1)
    return sum(vals)



if __name__ == "__main__":
    from AOC2022.python import run_solver

    run_solver(solve, __file__)

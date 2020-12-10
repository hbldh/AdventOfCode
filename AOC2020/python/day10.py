from collections import Counter

_data = """28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3"""

_data = """16
10
15
5
1
11
7
19
6
12
4"""

def solve_part_1(values):
    v = sorted(values)
    v.append(v[-1] + 3)
    v.insert(0, 0)
    diffs = [b - a for a, b in zip(v[:-1], v[1:])]
    c = Counter(diffs)
    return c[3] * c[1]


def solve_part_2(values):
    v = sorted(values)
    v.append(v[-1] + 3)
    v.insert(0, 0)

    paths = {v[-1]: 1}
    for a in v[:-1][::-1]:
        paths[a] = sum(paths.get(x, 0) for x in (a + 1, a + 2, a + 3))
    return paths[0]


def solve(data):
    values = list(map(int, data.splitlines()))
    return solve_part_1(values), solve_part_2(values)


if __name__ == "__main__":
    from AOC2020.python import run_solver

    run_solver(solve, __file__)

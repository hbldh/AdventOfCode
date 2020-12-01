import re


def _solve_1(initial_state, rules, n_generations=20):
    buffer = 500

    def _score(s, start=-buffer):
        return sum([i for i, status in enumerate(s, start=start) if status == '#'])

    state = '.' * buffer + initial_state.split(':')[-1].strip() + '.' * buffer

    for generation in range(n_generations):
        next_gen = ['.'] * len(state)
        for pattern, result in rules:
            for f in re.finditer(f'(?=({pattern}))', state):
                next_gen[f.start() + 2] = result
        new_state = ''.join(next_gen)
        if new_state[1:] == state[:-1]:
            return _score(new_state, -buffer + (n_generations - generation - 1))
        state = new_state
    return _score(state)


def solve(data):
    initial_state = data.splitlines()[0]
    rules = tuple(map(lambda x: x.split(' => '), data.splitlines()[2:]))
    rules = [(r[0].replace(".", "\."), r[1]) for r in rules]

    return _solve_1(initial_state, rules), _solve_1(initial_state, rules, n_generations=50000000000)


if __name__ == '__main__':
    from AOC2018 import run_solver
    run_solver(solve, __file__)

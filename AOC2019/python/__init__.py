import os
from urllib.request import build_opener

root_dir = os.path.dirname(os.path.abspath(__file__))


def ensure_data(day):
    day_input_file = os.path.join(root_dir, f'input_{day:02d}.txt')
    if not os.path.exists(day_input_file):
        session_token = os.environ.get('AOC_SESSION_TOKEN')
        if session_token is None:
            raise ValueError("Must set AOC_SESSION_TOKEN environment variable!")
        url = f'https://adventofcode.com/2019/day/{day}/input'
        opener = build_opener()
        opener.addheaders.append(('Cookie', f'session={session_token}'))
        response = opener.open(url)
        with open(day_input_file, 'w') as f:
            f.write(response.read().decode("utf-8"))


def load_data(day):
    with open(f'input_{day:02d}.txt', 'r') as f:
        data = f.read().strip()
    return data


def run_solver(f, file):
    day = int(file.split('/')[-1].strip('day').strip('.py'))
    ensure_data(day)
    data = load_data(day)

    part_1, part_2 = f(data)

    print("Part 1: {0}".format(part_1))
    print("Part 2: {0}".format(part_2))

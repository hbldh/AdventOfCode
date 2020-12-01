import datetime
from collections import defaultdict, Counter
import re
import operator


def solve(data):

    data = [(datetime.datetime.strptime(row[1:17], "%Y-%m-%d %H:%M"), row[19:].strip()) for row in data.splitlines()]
    data.sort(key=operator.itemgetter(0))

    slept_minutes = defaultdict(lambda: list())

    current_guard_session = None
    for dt, text in data:
        if text.startswith("Guard"):
            current_guard_session = [int(re.search('#(\d+)', text).groups()[0]), ]
        elif text.startswith("falls asleep"):
            current_guard_session.append(dt.minute)
        elif text.startswith("wakes up"):
            current_guard_session.append(dt.minute)
            slept_minutes[current_guard_session[0]] += list(range(current_guard_session[1], current_guard_session[2]))
            current_guard_session = [current_guard_session[0], ]
        else:
            raise RuntimeError("WTF?")

    n_minutes = [(k, len(v)) for k, v in slept_minutes.items()]
    n_minutes.sort(key=operator.itemgetter(1), reverse=True)
    most_common_minute = Counter(slept_minutes[n_minutes[0][0]]).most_common(1)[0][0]
    most_slept_minute_by_guard = sorted([(k, Counter(v).most_common(1)[0]) for k,v in slept_minutes.items()], key=lambda x: x[1][1], reverse=True)[0]
    return most_common_minute * n_minutes[0][0], most_slept_minute_by_guard[0] * most_slept_minute_by_guard[1][0]


if __name__ == '__main__':
    from AOC2018 import run_solver
    run_solver(solve, __file__)

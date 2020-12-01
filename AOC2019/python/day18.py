import copy
import functools
from collections import defaultdict


class Point:
    def __init__(self, x, y):
        self.xy = (x, y)

    def __add__(self, other):
        return Point(self.xy[0] + other.xy[0], self.xy[1] + other.xy[1])

    def __sub__(self, other):
        return Point(self.xy[0] - other.xy[0], self.xy[1] - other.xy[1])

    def __getitem__(self, ix):
        return self.xy[ix]

    def __repr__(self):
        return repr(self.xy)

    def __eq__(self, other):
        return self[0] == other[0] and self[1] == other[1]

    def __hash__(self):
        return self.xy.__hash__()



def reachable_keys(cur_distmap, keys):
    return {
        (pos, d, keydoor)
        for pos, d, keydoor, blockers in cur_distmap
        if keydoor not in keys and (keydoor == keydoor.lower() or keydoor.lower() in keys) and not (blockers - keys)
    }  # not visited and is key or openable door and reachable


DIRS = [Point(x, y) for x, y in [(-1, 0), (1, 0), (0, -1), (0, 1)]]


def distances(maze, pos):
    visited = defaultdict(bool)
    l = [(pos, frozenset(), 0)]
    visited[pos] = True
    keys_doors = []
    while l:
        nl = []
        for p, blockers, d in l:
            for dir in DIRS:
                newpos = p + dir
                sq = maze[newpos[1]][newpos[0]]
                if sq != "#" and not visited[newpos]:
                    visited[newpos] = True
                    new_blockers = blockers
                    if sq not in [".", "0", "1", "2", "3"]:
                        new_blockers = blockers | frozenset({sq})
                        keys_doors.append((newpos, d + 1, sq, blockers))  # dont include self as blocker
                    nl.append((newpos, new_blockers, d + 1))
        l = nl
    return keys_doors


def _solve(maze, robot_pos, allkeys, distmap):
    best_found = 1e9
    states = {(tuple(robot_pos), frozenset()): 0}
    iter = 0
    while states:
        iter += 1
        newstates = {}
        for (robot_positions, keys), dist in states.items():
            if not allkeys - keys:
                if dist < best_found:
                    best_found = dist
                continue
            for robot, pos in enumerate(robot_positions):
                cur = maze[pos[1]][pos[0]]
                keys_doors = reachable_keys(distmap[cur], keys)
                for newpos, d, keydoor in keys_doors:
                    newkeys = keys | frozenset({keydoor})
                    newposses = [*robot_positions]
                    newposses[robot] = newpos
                    newstate = (tuple(newposses), newkeys)
                    newstates[newstate] = min(newstates.get(newstate, 1e9), dist + d)
        states = newstates
    return best_found


def solve(data):
    output = []
    for _ in range(1,3):
        maze = [list(s.strip()) for s in data.splitlines()]
        if _ == 2:
            maze[39][39] = "@"
            maze[40][39] = "#"
            maze[41][39] = "@"
            maze[39][40] = "#"
            maze[40][40] = "#"
            maze[41][40] = "#"
            maze[39][41] = "@"
            maze[40][41] = "#"
            maze[41][41] = "@"
        allkeys = {sq for l in maze for sq in l if sq not in [".", "@", "#"] and sq == sq.lower()}
        robot_pos = [Point(x, y) for y, l in enumerate(maze) for x, _ in enumerate(l) if _ == "@"]
        for i, p in enumerate(robot_pos):
            maze[p[1]][p[0]] = str(i)
        distmap = {}
        for y, l in enumerate(maze):
            for x, sq in enumerate(l):
                if sq not in [".", "#"]:
                    distmap[sq] = distances(maze,Point(x, y))
        output.append(_solve(maze, robot_pos, allkeys, distmap))
    return tuple(output)

if __name__ == "__main__":
    from AOC2019.python import run_solver

    run_solver(solve, __file__)

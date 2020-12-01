#!/usr/bin/env python3

import heapq
import os
import pathlib
import sys

R_OK = 0
R_HALT = 1
R_INPUT = 2
R_OUTPUT = 3
R_INVALID = 99


class IntCode:
    def __init__(self, codes):
        self.code = {i: x for i, x in enumerate(codes)}
        self.ptr = 0
        self.relative_base = 0
        self.modes = [0, 0, 0]
        self.inputs = []
        self.outputs = []

    # Comparison functions
    # All IntCode instances are created equal
    def __eq__(self, other):
        return True

    def __ne__(self, other):
        return False

    def __lt__(self, other):
        return False

    def __le__(self, other):
        return True

    def __gt__(self, other):
        return False

    def __ge__(self, other):
        return True

    def inspect(self, pos):
        return self.code[pos]

    def edit(self, pos, val):
        self.code[pos] = val
        return self

    def write(self, input):
        self.inputs.append(input)
        return self

    def read(self):
        return self.outputs.pop(0)

    def run(self):
        res = R_OK
        while res == R_OK:
            res = self._step()
        return res

    def copy(self):
        new = IntCode([])

        new.code = self.code.copy()
        new.ptr = self.ptr
        new.relative_base = self.relative_base
        new.inputs = self.inputs.copy()
        new.outputs = self.outputs.copy()

        return new

    def _step(self):
        mode, op = divmod(self.code.get(self.ptr, 0), 100)
        self.ptr += 1
        self.modes = [int(x) for x in reversed(str(mode))] + [0, 0, 0]

        # opcode 1: add(arg1, arg2) -> arg3
        if op == 1:
            res = self._add()

        # opcode 2: multiply(arg1, arg2) -> arg2
        elif op == 2:
            res = self._mul()

        # opcode 3: read(input) -> arg1
        elif op == 3:
            res = self._read()

        # opcode 4: write(arg1) -> output
        elif op == 4:
            res = self._write()

        # opcode 5: if(arg1) -> jump(arg2)
        elif op == 5:
            res = self._jmp_if_true()

        # opcode 6: if(!arg1) -> jump(arg2)
        elif op == 6:
            res = self._jmp_if_false()

        # opcode 7: ifless(arg1, arg2) -> arg3
        elif op == 7:
            res = self._set_if_lt()

        # opcode 8: ifequal(arg1, arg2) -> arg3
        elif op == 8:
            res = self._set_if_eq()

        # opcode 9: base(arg1) -> relative_base
        elif op == 9:
            res = self._set_base()

        # opcode 99: halt
        elif op == 99:
            res = R_HALT

        # unknown opcode
        else:
            print(f"Invalid opcode {op}")
            res = R_INVALID

        return res

    def _next_value(self):
        return self._next_instr() if self.modes[0] == 1 else self.code.get(self._next_instr(), 0)

    def _next_instr(self):
        instr = self.code.get(self.ptr, 0)
        if self.modes.pop(0) == 2:
            instr += self.relative_base
        self.ptr += 1
        return instr

    def _jmp(self, pos):
        self.ptr = pos

    def _add(self):
        arg1 = self._next_value()
        arg2 = self._next_value()
        pos = self._next_instr()

        self.code[pos] = arg1 + arg2
        return R_OK

    def _mul(self):
        arg1 = self._next_value()
        arg2 = self._next_value()
        pos = self._next_instr()

        self.code[pos] = arg1 * arg2
        return R_OK

    def _read(self):
        if len(self.inputs) == 0:
            self.ptr -= 1
            return R_INPUT

        pos = self._next_instr()
        self.code[pos] = self.inputs.pop(0)
        return R_OK

    def _write(self):
        val = self._next_value()
        self.outputs.append(val)
        return R_OUTPUT

    def _jmp_if_true(self):
        val = self._next_value()
        pos = self._next_value()

        if val != 0:
            self._jmp(pos);

        return R_OK

    def _jmp_if_false(self):
        val = self._next_value()
        pos = self._next_value()

        if val == 0:
            self._jmp(pos);

        return R_OK

    def _set_if_lt(self):
        arg1 = self._next_value()
        arg2 = self._next_value()
        pos = self._next_instr()

        self.code[pos] = 1 if arg1 < arg2 else 0
        return R_OK

    def _set_if_eq(self):
        arg1 = self._next_value()
        arg2 = self._next_value()
        pos = self._next_instr()

        self.code[pos] = 1 if arg1 == arg2 else 0
        return R_OK

    def _set_base(self):
        self.relative_base += self._next_value()
        return R_OK

def move(p, d):
  return (p[0] + d[0], p[1] + d[1])


def droid(codes):
  dirs = {1: (0, 1), 2: (0, -1), 3: (-1, 0), 4: (1, 0)}

  pos = (0, 0)
  locs = {pos: " "}

  check = []
  min_steps = None

  computer = IntCode(codes)
  for x in dirs:
    heapq.heappush(check, (1, computer.copy(), move((0, 0), dirs[x]), x))

  while len(check):
    steps, computer, pos, mv = heapq.heappop(check)

    while True:
      res = computer.run()
      if res == R_HALT:
        pass

      elif res == R_INPUT:
        computer.write(mv)
        continue

      elif res == R_OUTPUT:
        status = computer.read()
        if status == 0:
          locs[pos] = "#"

        elif status == 1:
          locs[pos] = " "

          for mv in dirs:
            newpos = move(pos, dirs[mv])
            if newpos not in locs:
              heapq.heappush(check, (steps + 1, computer.copy(), newpos, mv))

        elif status == 2:
          locs[pos] = "O"
          if min_steps is None:
            min_steps = steps

      break

  return min_steps, locs

def fill_oxygen(locs):
  pos = next(x for x in locs if locs[x] == "O")
  fill = []
  heapq.heappush(fill, (0, pos))

  while len(fill):
    minutes, pos = heapq.heappop(fill)
    for adj in ((0, 1), (0, -1), (1, 0), (-1, 0)):
      adj_pos = move(pos, adj)
      if adj_pos in locs and locs[adj_pos] == " ":
        locs[adj_pos] = "O"
        heapq.heappush(fill, (minutes + 1, adj_pos))

  return minutes


def fmt_grid(locs):
  min_x = min(x for x, _ in locs.keys())
  min_y = min(y for _, y in locs.keys())
  max_x = max(x for x, _ in locs.keys())
  max_y = max(y for _, y in locs.keys())

  return "\n".join("".join(locs.get((x, y), "-") for x in range(min_x, max_x + 1, 1)) for y in range(min_y, max_y + 1, 1))


def run(data):

  program = data.strip()
  codes = [int(x) for x in program.split(",")]

  moves, locs = droid(codes)
  grid = fmt_grid(locs)

  print(f"Grid:\n{fmt_grid(locs)}\n")
  print(f"Moves required to find oxygen: {moves}")

  minutes = fill_oxygen(locs)
  print(f"Minutes to fill oxygen: {minutes}")
  return moves, minutes


def solve(data):
    run(data)


if __name__ == "__main__":
    from AOC2019.python import run_solver

    run_solver(solve, __file__)

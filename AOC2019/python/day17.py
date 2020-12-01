
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


MAX_REGISTER_LENGTH = 20


class Scaffold:
  dirs = { "^": -1j, "v": 1j, "<": -1, ">": 1}

  @classmethod
  def init_from_str(cls, s):
    self = Scaffold()

    rows = s.strip().split("\n")
    for y, row in enumerate(rows):
      for x, char in enumerate(row):
        p = x + y * 1j
        if char != ".":
          self.grid[p] = "#"
        if char in {"^", "v", "<", ">"}:
          self.bot = [p, self.dirs[char]]

    self.min_x = int(min([p.real for p in self.grid.keys()]))
    self.max_x = int(max([p.real for p in self.grid.keys()]))
    self.min_y = int(min([p.imag for p in self.grid.keys()]))
    self.max_y = int(max([p.imag for p in self.grid.keys()]))

    return self

  def __init__(self):
    self.grid = {}
    self.bot = None

  def __str__(self):
    dir_chars = {v: k for k, v in self.dirs.items()}
    return "\n".join(
        "".join(self.grid.get(x + y * 1j, " ")
        if self.bot[0] != x + y * 1j else dir_chars[self.bot[1]]
        for x in range(self.min_x, self.max_x + 1))
      for y in range(self.min_y, self.max_y + 1))

  def intersections(self):
    intersections = []

    for p in self.grid:
      adj = sum(1 for d in self.dirs.values() if p + d in self.grid)
      if adj == 4:
        intersections.append(p)

    return intersections

  def alignment_parameter(self, p):
    return int(p.real * p.imag)

  def path(self):
    path = []
    steps = 0

    while True:
      if self.bot[0] + self.bot[1] in self.grid:
        steps += 1
        self.bot[0] += self.bot[1]
      else:
        if len(path):
          path.append(str(steps))

        if self.bot[0] + self.bot[1] * -1j in self.grid:
          self.bot[1] *= -1j
          path.append("L")
          steps = 0
        elif self.bot[0] + self.bot[1] * 1j in self.grid:
          self.bot[1] *= 1j
          path.append("R")
          steps = 0
        else:
          break

    return path


def get_scaffold(codes):
  data = ""
  computer = IntCode(codes)

  while True:
    res = computer.run()
    if res == R_HALT:
      break

    elif res == R_OUTPUT:
      data += chr(computer.read())

  return Scaffold.init_from_str(data)


def drive_bot(codes, main, registers):
  computer = IntCode(codes)
  computer.edit(0, 2)

  inputs = \
    [ord(x) for x in main] + [ord("\n")] + \
    [ord(x) for x in registers[0]] + [ord("\n")] + \
    [ord(x) for x in registers[1]] + [ord("\n")] + \
    [ord(x) for x in registers[2]] + [ord("\n")] + \
    [ord("n")] + [ord("\n")]

  while True:
    res = computer.run()
    if res == R_HALT:
      break

    elif res == R_INPUT:
      computer.write(inputs.pop(0))

    elif res == R_OUTPUT:
      data = computer.read()

  return data


def find_repeat(path, registers=[], sequence=[]):
  cleared = False
  while not cleared:
    cleared = True

    for i, prev in enumerate(registers):
      if len(prev) <= len(path) and path[:len(prev)] == prev:
        path = path[len(prev):]
        sequence.append(i)
        cleared = False
        break

  # last run
  # either we have consumed all of our path
  # or this is a dead-end
  if len(registers) == 3:
    return (True, registers, sequence) if len(path) == 0 else (False, None, None)

  register_len = min(len(path), MAX_REGISTER_LENGTH // 2)

  # our string form of the path must fit within our register constraint
  # we start on a turn, so we do not want to end on a turn
  # repeats could then be (turn, turn, #), which is not an efficient sequence
  while len(",".join(path[:register_len])) > MAX_REGISTER_LENGTH \
     or path[register_len - 1] in {'R', 'L'}:
    register_len -= 1

  while register_len > 0:
    res, matches, seq = find_repeat(path, registers + [path[:register_len]], sequence.copy())
    if res:
      return res, matches, seq
    register_len -= 2

  return False, [], []


def run(program):
  MAX_REGISTER = 20
  codes = [int(x) for x in program.split(",")]

  scaffold = get_scaffold(codes)
  print(f"{scaffold}\n")

  intersections = scaffold.intersections()
  alignments = sum(scaffold.alignment_parameter(x) for x in intersections)
  print(f"Sum of alignment parameters {alignments}")

  _, registers, sequence = find_repeat(scaffold.path())
  main = ",".join(chr(x + ord('A')) for x in sequence)
  regcode = [",".join(x) for x in registers]

  dust = drive_bot(codes, main, regcode)
  print(f"Dust collected: {dust}")



def solve(data):
    return 0, run(data.strip())


if __name__ == "__main__":
    from AOC2019.python import run_solver

    run_solver(solve, __file__)

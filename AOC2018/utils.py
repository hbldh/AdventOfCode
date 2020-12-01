import itertools

def is_iterable(obj):
    """Similar in nature to :func:`callable`, ``is_iterable`` returns
    ``True`` if an object is `iterable`_, ``False`` if not.

    >>> is_iterable([])
    True
    >>> is_iterable(object())
    False

    .. _iterable: https://docs.python.org/2/glossary.html#term-iterable
    """
    try:
        iter(obj)
    except TypeError:
        return False
    return True

def chunked(src, size, count=None, **kw):
    """Returns a list of *count* chunks, each with *size* elements,
    generated from iterable *src*. If *src* is not evenly divisible by
    *size*, the final chunk will have fewer than *size* elements.
    Provide the *fill* keyword argument to provide a pad value and
    enable padding, otherwise no padding will take place.

    >>> chunked(range(10), 3)
    [[0, 1, 2], [3, 4, 5], [6, 7, 8], [9]]
    >>> chunked(range(10), 3, fill=None)
    [[0, 1, 2], [3, 4, 5], [6, 7, 8], [9, None, None]]
    >>> chunked(range(10), 3, count=2)
    [[0, 1, 2], [3, 4, 5]]

    See :func:`chunked_iter` for more info.
    """
    chunk_iter = chunked_iter(src, size, **kw)
    if count is None:
        return list(chunk_iter)
    else:
        return list(itertools.islice(chunk_iter, count))


def chunked_iter(src, size, **kw):
    """Generates *size*-sized chunks from *src* iterable. Unless the
    optional *fill* keyword argument is provided, iterables not even
    divisible by *size* will have a final chunk that is smaller than
    *size*.

    >>> list(chunked_iter(range(10), 3))
    [[0, 1, 2], [3, 4, 5], [6, 7, 8], [9]]
    >>> list(chunked_iter(range(10), 3, fill=None))
    [[0, 1, 2], [3, 4, 5], [6, 7, 8], [9, None, None]]

    Note that ``fill=None`` in fact uses ``None`` as the fill value.
    """
    # TODO: add count kwarg?
    if not is_iterable(src):
        raise TypeError('expected an iterable')
    size = int(size)
    if size <= 0:
        raise ValueError('expected a positive integer chunk size')
    do_fill = True
    try:
        fill_val = kw.pop('fill')
    except KeyError:
        do_fill = False
        fill_val = None
    if kw:
        raise ValueError('got unexpected keyword arguments: %r' % kw.keys())
    if not src:
        return
    postprocess = lambda chk: chk
    if isinstance(src, str):
        postprocess = lambda chk, _sep=type(src)(): _sep.join(chk)
    src_iter = iter(src)
    while True:
        cur_chunk = list(itertools.islice(src_iter, size))
        if not cur_chunk:
            break
        lc = len(cur_chunk)
        if lc < size and do_fill:
            cur_chunk[lc:] = [fill_val] * (size - lc)
        yield postprocess(cur_chunk)
    return

## Why Debug Chan?

For complex projects “just logging to the console” is helpful but also a time waster; logging to the console either means a "free for all" approach (much junk to the console) or a zero trace policy (very little information to frame incoming issues).

Debug Chan is meant to support a principled approach, where developers write (relatively) humanly readable and much less transient traces to document program execution.

As such, debug chan implements three essential features:

**Channeled logging** - select a game object or component to view matching traces.

**Frame logging** - frame based processes often exhibit "stable" frame ranges. When an object look 'stuck', more likely than not they are cycling through the same trace over and over.

**Channeled histories** - view an object's recent (or not so recent) history, aggregated via frame ranges.

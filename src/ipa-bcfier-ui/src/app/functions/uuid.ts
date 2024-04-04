import { v4 as uuidv4 } from 'uuid';

export function getNewRandomGuid(): string {
  if (crypto && crypto.randomUUID) {
    return crypto.randomUUID();
  }

  return uuidv4();
}

export interface Duck {
  id: number;
  name: string;
  speciality: string;
  emoji: string;
}

export interface Consultation {
  id: string;
  duckId: number;
  duckName: string;
  duckEmoji: string;
  problem: string;
  tip: string;
  askedAt: string;
}

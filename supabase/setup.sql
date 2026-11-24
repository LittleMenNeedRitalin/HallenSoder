-- Kör detta en gång i Supabase: SQL Editor -> New query -> klistra in -> Run.

create table if not exists public.events (
  id          uuid primary key default gen_random_uuid(),
  type        text not null default 'Annat',
  title       text not null,
  starts_at   timestamptz not null,
  league      text,
  description text,
  bookable    boolean not null default true,
  featured    boolean not null default false,
  created_at  timestamptz not null default now()
);

-- Rättigheter för API:et (nyare Supabase-projekt ger dem inte automatiskt).
grant usage on schema public to anon, authenticated;
grant select on public.events to anon, authenticated;
grant insert, update, delete on public.events to authenticated;

alter table public.events enable row level security;

-- Alla (även besökare) får läsa event.
create policy "events_select_public" on public.events
  for select to anon, authenticated using (true);

-- Bara inloggad personal får skapa, ändra och ta bort.
create policy "events_insert_staff" on public.events
  for insert to authenticated with check (true);

create policy "events_update_staff" on public.events
  for update to authenticated using (true) with check (true);

create policy "events_delete_staff" on public.events
  for delete to authenticated using (true);

-- VIKTIGT efter detta steg (i Supabase-dashboarden):
--   Authentication -> Sign In / Providers -> Email -> stäng av "Allow new users to sign up".
-- Annars kan vem som helst skapa ett konto och därmed få skrivrätt.
-- Personalen läggs i stället till manuellt under Authentication -> Users -> Add user.

-- ============================================================
-- BILDER PÅ EVENT (lägg till om du redan kört skriptet ovan)
-- ============================================================
alter table public.events add column if not exists image_url text;

insert into storage.buckets (id, name, public)
values ('event-images', 'event-images', true)
on conflict (id) do nothing;

drop policy if exists "event_images_insert_staff" on storage.objects;
drop policy if exists "event_images_delete_staff" on storage.objects;
drop policy if exists "event_images_read_public" on storage.objects;

create policy "event_images_read_public" on storage.objects
  for select to anon, authenticated using (bucket_id = 'event-images');
create policy "event_images_insert_staff" on storage.objects
  for insert to authenticated with check (bucket_id = 'event-images');
create policy "event_images_delete_staff" on storage.objects
  for delete to authenticated using (bucket_id = 'event-images');

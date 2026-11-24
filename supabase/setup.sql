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

-- ============================================================
-- MENY OCH TEXTER (redigerbart i admin)
-- ============================================================
create table if not exists public.menu_sections (
  id    uuid primary key default gen_random_uuid(),
  slug  text not null,
  title text not null,
  note  text,
  sort  int  not null default 0
);

create table if not exists public.menu_items (
  id          uuid primary key default gen_random_uuid(),
  section_id  uuid not null references public.menu_sections(id) on delete cascade,
  name        text not null,
  description text,
  price       int,
  sort        int  not null default 0,
  hidden      boolean not null default false
);

create table if not exists public.site_content (
  key        text primary key,
  value      text,
  updated_at timestamptz not null default now()
);

grant select on public.menu_sections, public.menu_items, public.site_content to anon, authenticated;
grant insert, update, delete on public.menu_sections, public.menu_items, public.site_content to authenticated;

alter table public.menu_sections enable row level security;
alter table public.menu_items    enable row level security;
alter table public.site_content  enable row level security;

drop policy if exists "menu_sections_select" on public.menu_sections;
drop policy if exists "menu_sections_insert" on public.menu_sections;
drop policy if exists "menu_sections_update" on public.menu_sections;
drop policy if exists "menu_sections_delete" on public.menu_sections;
create policy "menu_sections_select" on public.menu_sections for select to anon, authenticated using (true);
create policy "menu_sections_insert" on public.menu_sections for insert to authenticated with check (true);
create policy "menu_sections_update" on public.menu_sections for update to authenticated using (true) with check (true);
create policy "menu_sections_delete" on public.menu_sections for delete to authenticated using (true);

drop policy if exists "menu_items_select_public" on public.menu_items;
drop policy if exists "menu_items_select_staff"  on public.menu_items;
drop policy if exists "menu_items_insert" on public.menu_items;
drop policy if exists "menu_items_update" on public.menu_items;
drop policy if exists "menu_items_delete" on public.menu_items;
create policy "menu_items_select_public" on public.menu_items for select to anon using (not hidden);
create policy "menu_items_select_staff"  on public.menu_items for select to authenticated using (true);
create policy "menu_items_insert" on public.menu_items for insert to authenticated with check (true);
create policy "menu_items_update" on public.menu_items for update to authenticated using (true) with check (true);
create policy "menu_items_delete" on public.menu_items for delete to authenticated using (true);

drop policy if exists "site_content_select" on public.site_content;
drop policy if exists "site_content_insert" on public.site_content;
drop policy if exists "site_content_update" on public.site_content;
drop policy if exists "site_content_delete" on public.site_content;
create policy "site_content_select" on public.site_content for select to anon, authenticated using (true);
create policy "site_content_insert" on public.site_content for insert to authenticated with check (true);
create policy "site_content_update" on public.site_content for update to authenticated using (true) with check (true);
create policy "site_content_delete" on public.site_content for delete to authenticated using (true);
